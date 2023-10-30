using System.Net.Sockets;
using System.Text;
using System.Text.Json;

using EventBus;
using EventBus.Events;
using EventBus.Extensions;
using EventBus.Interfaces;

using EventBusRabbitMQ.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Polly;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ;

public class EventBusDefault : IEventBus, IDisposable
{
	private readonly IRabbitMQPersistentConnection _persistentConnection;
	private readonly ILogger<EventBusDefault> _logger;
	private readonly IEventBusSubscriptionsManager _subsManager;
	private readonly IServiceProvider _serviceProvider;
	private readonly IEventBusDeadLetter _deadLetter;
	private readonly IRabbitMQBrokerConfig _config;

	private readonly CancellationTokenSource _tokenSource;

	private IModel _consumerChannel;

	public EventBusDefault(
		IRabbitMQPersistentConnection persistentConnection,
		ILogger<EventBusDefault> logger,
		IServiceProvider serviceProvider,
		IEventBusSubscriptionsManager subsManager,
		IEventBusDeadLetter deadLetter,
		IRabbitMQBrokerConfig config,
		CancellationTokenSource? tokenSource = null)
	{
		_persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_subsManager = subsManager ?? new InMemorySubscriptionsManager();
		_serviceProvider = serviceProvider;
		_deadLetter = deadLetter;
		_config = config;
		_tokenSource = tokenSource ?? new CancellationTokenSource();
		_consumerChannel = CreateConsumerChannel();
		_subsManager.OnEventRemoved += SubsManager_OnEventRemoved!;
	}

	public void Publish(IntegrationEvent @event)
	{
		if (!_persistentConnection.IsConnected)
			_persistentConnection.TryConnect();

		var policy = Policy
			.Handle<BrokerUnreachableException>()
			.Or<SocketException>()
			.WaitAndRetry(_config.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
			{
				_logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s", @event.Id, $"{time.TotalSeconds:n1}");
			});

		var eventName = @event.GetType().Name;

		_logger.LogTrace("Publish event: {EventId} ({EventName})", @event.Id, eventName);

		_consumerChannel.ExchangeDeclare(
			exchange: _config.Broker,
			type: _config.ExchangeType,
			durable: true);

		_logger.LogTrace("Exchange to publish event: {EventId}", @event.Id);

		var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
		{
			WriteIndented = true
		});

		policy.Execute(() =>
		{
			var properties = _consumerChannel.CreateBasicProperties();

			//properties.DeliveryMode = 2; // persistent
			properties.Persistent = true; // DeliveryMode = 2, 
			properties.Headers = _deadLetter.Headers;

			_logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

			_consumerChannel.BasicPublish(
				exchange: _config.Broker,
				routingKey: eventName,
				mandatory: true,
				basicProperties: properties,
				body: body);
		});
	}

	public void SubscribeDynamic<TH>(string eventName)
		where TH : IDynamicIntegrationEventHandler
	{
		_logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

		DoInternalSubscription(eventName);
		_subsManager.AddDynamicSubscription<TH>(eventName);
		StartBasicConsume();
	}

	public void Subscribe<T, TH>()
		where T : IntegrationEvent
		where TH : IIntegrationEventHandler<T>
	{
		var eventName = _subsManager.GetEventKey<T>();
		DoInternalSubscription(eventName);

		_logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

		_subsManager.AddSubscription<T, TH>();
		StartBasicConsume();
	}

	public void Unsubscribe<T, TH>()
		where T : IntegrationEvent
		where TH : IIntegrationEventHandler<T>
	{
		var eventName = _subsManager.GetEventKey<T>();

		_logger.LogInformation("Unsubscribing from event {EventName}", eventName);

		_subsManager.RemoveSubscription<T, TH>();
	}

	public void UnsubscribeDynamic<TH>(string eventName)
		where TH : IDynamicIntegrationEventHandler
	{
		_subsManager.RemoveDynamicSubscription<TH>(eventName);
	}

	public void Dispose()
	{
		_tokenSource.Cancel();

		if (_consumerChannel != null)
			_consumerChannel.Dispose();

		_subsManager.Clear();
	}

	/// <summary>
	/// Обработчик удаления подписки
	/// </summary>
	private void SubsManager_OnEventRemoved(object sender, string eventName)
	{
		if (!_persistentConnection.IsConnected)
			_persistentConnection.TryConnect();

		_consumerChannel.QueueUnbind(queue: _config.QueueName,
			exchange: _config.Broker,
			routingKey: eventName);

		if (_subsManager.IsEmpty)
		{
			_config.QueueName = string.Empty;
			_consumerChannel.Close();
		}
	}

	/// <summary>
	/// Добавить обработчик на событие
	/// </summary>
	/// <param name="eventName"></param>
	private void DoInternalSubscription(string eventName)
	{
		var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
		if (!containsKey)
		{
			if (!_persistentConnection.IsConnected)
				_persistentConnection.TryConnect();

			_consumerChannel.QueueBind(queue: _config.QueueName,
								exchange: _config.Broker,
								routingKey: eventName);
		}
	}

	/// <summary>
	/// Запустить получение данных из созданного канала через объявленную очередь
	/// </summary>
	private void StartBasicConsume()
	{
		_logger.LogTrace("Starting RabbitMQ basic consume");

		if (_consumerChannel != null)
		{
			_consumerChannel.BasicQos(0, (ushort)_config.PrefetchCount, false);

			var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

			consumer.Received += Consumer_Received;

			_consumerChannel.BasicConsume(
				queue: _config.QueueName,
				autoAck: false,
				consumer: consumer);
		}
		else
			_logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
	}

	/// <summary>
	/// Обработчик полученного из очереди сообщения
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
	{
		var eventName = eventArgs.RoutingKey;
		var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

		try
		{
			if (message.ToLowerInvariant().Contains("throw-fake-exception"))
				throw new InvalidOperationException($"Fake exception requested: \"{message}\"");

			await ProcessEvent(eventName, message);

			_consumerChannel.BasicAck(
				eventArgs.DeliveryTag,
				multiple: false);
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error Processing message \"{Message}\"", message);

			_consumerChannel.BasicNack(
				eventArgs.DeliveryTag,
				multiple: false,
				requeue: false);
		}
	}

	/// <summary>
	/// Создать канал для подписчика, должен быть один для каждого соединения/потока!
	/// Следует избегать использования экземпляра IModel более чем одним потоком одновременно
	/// </summary>
	private IModel CreateConsumerChannel()
	{
		if (!_persistentConnection.IsConnected)
			_persistentConnection.TryConnect();

		_logger.LogTrace("Creating RabbitMQ consumer channel");

		var channel = _persistentConnection.CreateModel();

		// запускаем обменник недоставленных сообщений в фоновом потоке
		Task.Run(() =>
		{
			try
			{
				_logger.LogTrace("Start Dead Letter RabbitMQ");
				_deadLetter.StartDeadLetter(_tokenSource.Token);
			}
			catch (Exception ex)
			{
				_logger.LogError("Ошибка во время работы обменника недоставленных событий. {ex}", ex);
				throw;
			}
		});

		channel.ExchangeDeclare(
			exchange: _config.Broker,
			type: _config.ExchangeType,
			durable: true);

		channel.QueueDeclare(
			queue: _config.QueueName,
			durable: true,
			exclusive: true,
			autoDelete: false,
			arguments: _deadLetter.Headers); // устанавливаем свзяь с обменником недоставленных сообщений, эти же аргументы надо передать и при отправке сообщения

		channel.CallbackException += (sender, ea) =>
		{
			_logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

			_consumerChannel.Dispose();
			_consumerChannel = CreateConsumerChannel();
			StartBasicConsume();
		};

		return channel;
	}

	/// <summary>
	/// Процесс обработкти полученного сообщения
	/// </summary>
	/// <param name="eventName">событие</param>
	/// <param name="message">сообщение</param>
	/// <returns></returns>
	private async Task ProcessEvent(string eventName, string message)
	{
		_logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

		if (_subsManager.HasSubscriptionsForEvent(eventName))
		{
			await using var scope = _serviceProvider.CreateAsyncScope();

			var subscriptions = _subsManager.GetHandlersForEvent(eventName);

			foreach (var subscription in subscriptions)
			{
				if (subscription.IsDynamic)
				{
					if (scope.ServiceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler)
						continue;

					using dynamic eventData = JsonDocument.Parse(message);

					await Task.Yield();

					await handler.Handle(eventData);
				}
				else
				{
					var handler = scope.ServiceProvider.GetService(subscription.HandlerType);

					if (handler == null) continue;

					var eventType = _subsManager.GetEventTypeByName(eventName);

					var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = true
					});

					var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

					await Task.Yield();

					await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { integrationEvent! })!;
				}
			}
		}
		else
		{
			var errorMessage = "No subscription for RabbitMQ event";
			_logger.LogError("{ErrorMessage}: {EventName}", errorMessage, eventName);
			throw new ArgumentException(errorMessage, nameof(eventName));
		}
	}
}
