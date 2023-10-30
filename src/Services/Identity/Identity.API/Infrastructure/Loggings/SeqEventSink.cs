using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;

using Serilog;
using Serilog.Core;

using Service.Common.Extensions;

namespace Identity.API.Infrastructure.Loggings;

public class SeqEventSink : IEventSink
{
	private readonly Logger _log;
	private readonly IConfiguration _config;

	public SeqEventSink(IConfiguration config)
	{
		_config = config;
		_log = new LoggerConfiguration()
			.WriteTo.Seq(_config.GetRequiredValue("SeqAddress"))
			.CreateLogger();
	}

	public Task PersistAsync(Event evt)
	{
		if (evt.EventType == EventTypes.Success ||
			evt.EventType == EventTypes.Information)
		{
			_log.Information("{Name} ({Id}), Details: {@details}",
				evt.Name,
				evt.Id,
				evt);
		}
		else
		{
			_log.Error("{Name} ({Id}), Details: {@details}",
				evt.Name,
				evt.Id,
				evt);
		}

		return Task.CompletedTask;
	}
}