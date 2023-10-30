using System.Collections.Concurrent;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Notification.API.SignalR.Hubs;

[Authorize]
public class NotificationHub : Hub
{
	private readonly ILogger<NotificationHub> _logger;
	private readonly string _group;

	/// <summary>
	/// Потокобезопасная коллекция клиентов пользователя хаба SignalR
	/// </summary>
	public static ConcurrentDictionary<string, string> UserConnections = new();

	public NotificationHub(IConfiguration configuration, Func<IConfiguration, string> func, ILogger<NotificationHub> logger)
	{
		_group = func(configuration);
		_logger = logger;
	}

	public override async Task OnConnectedAsync()
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, _group);

		var userName = Context.User?.FindFirst("name")?.Value!;
		var result = UserConnections.TryAdd(Context.ConnectionId, userName!);

		if (!result)
			throw new InvalidOperationException($"Не удалось добавить идентификатор соединения {Context.ConnectionId} в коллекцию соединений пользователей хаба" +
				$"для пользователя {userName}");

		_logger.LogInformation("Пользователь {userName} вошёл в группу {group}", userName, _group);
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception ex)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, _group);

		var item = UserConnections
			.Where(c => c.Key == Context.ConnectionId)
			.Single();

		UserConnections.TryRemove(item);

		_logger.LogInformation("Пользователь {userName} вышел из группы {group}", item.Value, _group);
		await base.OnDisconnectedAsync(ex);
	}
}
