using System.Text;
using System.Text.Json;

using EventBus;
using EventBus.Events;
using EventBus.Extensions;
using EventBus.Interfaces;

using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBusRabbitMQ;

public class EventBusDeadLetter : IEventBusDeadLetter, IDisposable
{
	private readonly IRabbitMQPersistentConnection _persistentConnection;
	private readonly ILogger<EventBusDeadLetter> _logger;
	private readonly IEventBusSubscriptionsManager _subsManager;
	private readonly IServiceProvider _serviceProvider;
	private readonly IRabbitMQDlxBrokerConfig _config;

	private IModel _consumerChannel;

	public IDictionary<string, object> Headers { get; set; }

	public EventBusDeadLetter(
		IRabbitMQPersistentConnection persistentConnection,
		ILogger<EventBusDeadLetter> logger,
		IServiceProvider serviceProvider,
		IEventBusSubscriptionsManager subsManager,
		IRabbitMQDlxBrokerConfig config)
	{
		_persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
		_subsManager = subsManager ?? new InMemorySubscriptionsManager();
		_serviceProvider = serviceProvider;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_config = config;
		Headers = new Dictionary<string, object>()
		{
			{ "x-dead-letter-exchange", _config.Broker },
			{ "x-dead-letter-routing-key", _config.DlxRoutingKey }
		};
	}

	public void StartDeadLetter(CancellationToken cancel = default)
	{
		_consumerChannel = CreateConsumerChannel();
		_subsManager.OnEventRemoved += SubsManager_OnEventRemoved!;
	}

	public void Subscribe<T, TH>()
		where T : IntegrationEvent
		where TH : IIntegrationEventHandler<T>
	{
		var eventName = _subsManager.GetEventKey<T>();
		DoInternalSubscription(eventName);

		_logger.LogInformation("DLX Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

		_subsManager.AddSubscription<T, TH>();
		StartBasicConsume();
	}

	public void Unsubscribe<T, TH>()
		where T : IntegrationEvent
		where TH : IIntegrationEventHandler<T>
	{
		var eventName = _subsManager.GetEventKey<T>();

		_logger.LogInformation("DLX Unsubscribing from event {EventName}", eventName);

		_subsManager.RemoveSubscription<T, TH>();
	}

	public void Dispose()
	{
		if (_consumerChannel != null)
			_consumerChannel.Dispose();

		_subsManager.Clear();
	}

	/// <summary>
	/// Публикация события в очередь недоставленных событий
	/// </summary>
	private void Publish(DlxIntegrationEvent @event, IBasicProperties? properties = null)
	{
		var eventName = @event.GetType().Name;

		_logger.LogTrace("DLX Publish dlxEvent: {EventId} ({EventName}), retryEvent: {RetryEventId}", @event.Id, eventName, @event.EventId);

		_consumerChannel.ExchangeDeclare(
			exchange: _config.Broker,
			type: _config.ExchangeType,
			durable: true);

		_logger.LogTrace("DLX Exchange to publish event: {EventId}, retryEvent: {RetryEventId}", @event.Id, @event.EventId);

		var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
		{
			WriteIndented = true
		});

		if (properties is null)
		{
			properties = _consumerChannel.CreateBasicProperties();
			//properties.DeliveryMode = 2; // persistent
			properties.Persistent = true; // DeliveryMode = 2, 

			// Укажите заголовок x-delay в сообщениях, используя последовательное время задержки,
			// чтобы избежать перегрузки RabbitMQ. Это позволяет упростить процесс обработки недоставленных сообщений.
			properties.Headers = new Dictionary<string, object>()
			{
				{ "x-delay", TimeSpan.FromSeconds(_config.XDelay).TotalSeconds }
			};
		}

		_logger.LogTrace("DLX Publishing event to RabbitMQ: {Event}, retryEventId: {RetryEvent}", @event.Id);

		_consumerChannel.BasicPublish(
			exchange: _config.Broker,
			routingKey: eventName,
			mandatory: true,
			basicProperties: properties,
			body: body);
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
	/// Создать канал для подписчика, должен быть один для каждого соединения/потока!
	/// Следует избегать использования экземпляра IModel более чем одним потоком одновременно
	/// </summary>
	private IModel CreateConsumerChannel()
	{
		if (!_persistentConnection.IsConnected)
			_persistentConnection.TryConnect();

		_logger.LogTrace("DLX Creating RabbitMQ consumer channel");

		var channel = _persistentConnection.CreateModel();

		channel.ExchangeDeclare(
			exchange: _config.Broker,
			type: _config.ExchangeType,
			durable: true);

		channel.QueueDeclare(
			queue: _config.QueueName,
			durable: true,
			exclusive: true,
			autoDelete: false,
			arguments: null);

		channel.CallbackException += (sender, ea) =>
		{
			_logger.LogWarning(ea.Exception, "DLX Recreating RabbitMQ consumer channel");

			_consumerChannel.Dispose();
			_consumerChannel = CreateConsumerChannel();
			StartBasicConsume();
		};

		return channel;
	}

	/// <summary>
	/// Запустить получение данных из созданного канала через объявленную очередь
	/// </summary>
	private void StartBasicConsume()
	{
		_logger.LogTrace("DLX Starting RabbitMQ basic consume");

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
			_logger.LogError("DLX StartBasicConsume can't call on _consumerChannel == null");
	}

	/// <summary>
	/// Обработчик полученного из очереди сообщения
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
	{
		var eventName = eventArgs.RoutingKey;

		var retryMessage = Encoding.UTF8.GetString(eventArgs.Body.Span);
		var retryEventId = GetRetryEventId(retryMessage);
		var dlxEvent = GetDlxIntegrationEvent(retryEventId);

		try
		{
			await ProcessEvent(eventName, dlxEvent);

			_consumerChannel.BasicAck(
				eventArgs.DeliveryTag,
				multiple: false);
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "DLX Error Processing message \"{Message}\"", retryMessage);

			_consumerChannel.BasicNack(
				eventArgs.DeliveryTag,
				multiple: false,
				requeue: false);

			await Task.Delay(TimeSpan.FromSeconds(_config.XDelay)).ConfigureAwait(false);

			// мы должны снова отправить сообщение потребителю, тк мы уже в обменнике недоставленных сообщений.
			// потребителями у нас являются воркеры, которые отправляют события в "обычные" обменники () и следят за их успешностью обработки
			// обменник недоставленных сообщений сам делает публикации либо обратно в исходную шину, либо в воркере отправки сообщений, чтобы тот сделал снова публикацию
			// терять сообщения нам НЕЛЬЗЯ
			Publish(dlxEvent);
		}
	}

	private Guid GetRetryEventId(string retryMessage)
	{
		var jsonObject = JObject.Parse(retryMessage);
		// если сообщение попало первый раз в обменник недоставленных сообщений, то тело его содержит событие не типа DlxIntegrationEvent, а значит 
		// извлекаем Id, если собщение пришло повторно, те уже после того как был создан тип DlxIntegrationEvent, то извлекаем EventId 
		var eventIdObject = jsonObject["EventId"] ?? jsonObject["Id"];
		return (Guid)eventIdObject!;
	}

	private DlxIntegrationEvent GetDlxIntegrationEvent(Guid eventId)
	{
		return new DlxIntegrationEvent(eventId);
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
	/// Процесс обработкти полученного сообщения из очереди недоставленных сообщений
	/// </summary>
	/// <param name="eventName">событие</param>
	/// <param name="message">сообщение</param>
	/// <returns></returns>
	private async Task ProcessEvent(string eventName, DlxIntegrationEvent dlxEvent)
	{
		_logger.LogTrace("DLX Processing RabbitMQ event: {EventName}", eventName);

		if (_subsManager.HasSubscriptionsForEvent(eventName))
		{
			await using var scope = _serviceProvider.CreateAsyncScope();

			var subscriptions = _subsManager.GetHandlersForEvent(eventName);

			foreach (var subscription in subscriptions)
			{
				var handler = scope.ServiceProvider.GetService(subscription.HandlerType);

				if (handler == null) continue;

				var eventType = _subsManager.GetEventTypeByName(eventName);

				var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

				await Task.Yield();

				await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { dlxEvent! })!;
			}
		}
		else
		{
			var errorMessage = "No subscription for DLX RabbitMQ event";
			_logger.LogError("{ErrorMessage}: {EventName}", errorMessage, eventName);
			throw new ArgumentException(errorMessage, nameof(eventName));
		}
	}
}
