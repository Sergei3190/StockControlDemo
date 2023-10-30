using Microsoft.Extensions.Configuration;

namespace Service.Common.Configs;

public static class Config
{
	public static IConfiguration Configuration { get; private set; }

	public static IConfiguration GetConfiguration()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile($"Configs/appsettings.json", true, false);

		Configuration = builder.Build();

		return Configuration;
	}
}