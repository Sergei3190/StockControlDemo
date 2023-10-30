namespace IntegrationEventLogEF;

public record EventLogDto(Guid EventId, DateTimeOffset CreationTime, string EventTypeName)
{
}