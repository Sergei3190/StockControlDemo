namespace Service.Common.Settings;

internal class CorsSettings
{
    public string? Name { get; set; }
	public string[] ExposedHeaders { get; set; }
	public string[] AllowedOriginsList { get; set; }
    public string[] AllowedMethodsList { get; set; }
    public string[] AllowedHeadersList { get; set; }
}