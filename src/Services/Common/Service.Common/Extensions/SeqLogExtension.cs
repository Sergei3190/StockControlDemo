using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace Service.Common.Extensions;

public static class SeqLogExtension
{
	public static IHostBuilder AddDefaultSeqLog(this IHostBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));

		builder.UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
		   .MinimumLevel.Debug()
		   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
		   .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
		   .MinimumLevel.Override("Dapper", LogEventLevel.Warning)
		   .MinimumLevel.Override("Microsoft.Extensions.Diagnostics.HealthChecks.DefaultHealthCheckService", LogEventLevel.Warning)
		   .Enrich.FromLogContext()
		   .WriteTo.Console(
				outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
		   .WriteTo.Seq(host.Configuration.GetRequiredValue("SeqAddress"))
		);

		return builder;
	}
}
