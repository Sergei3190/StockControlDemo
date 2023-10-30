namespace FileStorage.API.Infrastructure.Settings;

public class KestrelLimitSettings
{
	public const string SectionName = "Kestrel:Limits";
    //для возможности использовать значение в атрибуте контроллера
	public const long MaxRequestBodySize = 1073741824;

    public int KeepAliveTimeout { get; set; }
    public int RequestHeadersTimeout { get; set; }
}
