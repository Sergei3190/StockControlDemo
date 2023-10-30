namespace Identity.API.Configuration;

internal static class ApiScopeDefinitions
{
	public static (string name, string displayName) WebBffStockControl { get; } = ("web.bff.stockcontrol", "Web Stock Control Aggregator");
	public static (string name, string displayName) StockControl { get; } = ("stock.control", "Stock Control Service");
	public static (string name, string displayName) Note { get; } = ("note", "Note Service");
	public static (string name, string displayName) NoteGrpc { get; } = ("note.grpc", "Note Grpc Service");
	public static (string name, string displayName) Notification { get; } = ("notification", "Notification Service");
	public static (string name, string displayName) PersonalCabinet { get; } = ("personal.cabinet", "Personal Cabinet Service");
	public static (string name, string displayName) FileStorage { get; } = ("file.storage", "File Storage Service");
}