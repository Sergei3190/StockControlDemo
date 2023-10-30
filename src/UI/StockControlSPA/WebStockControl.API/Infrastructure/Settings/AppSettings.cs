namespace WebStockControl.API.Infrastructure.Settings;

public class AppSettings
{
	public string BffUrl { get; set; }
	public string IdentityUrl { get; set; }

	public override string ToString() => $"{nameof(BffUrl)}: {BffUrl} {nameof(IdentityUrl)}: {IdentityUrl}";
}