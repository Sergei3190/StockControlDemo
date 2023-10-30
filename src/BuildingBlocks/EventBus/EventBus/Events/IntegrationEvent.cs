using System.Text.Json.Serialization;

namespace EventBus.Events;

/// <summary>
/// Базовый класс событий
/// </summary>
public class IntegrationEvent
{
	[JsonInclude]
	public Guid Id { get; private init; }

	[JsonInclude]
	public DateTimeOffset CreationDate { get; private init; }

	public IntegrationEvent()
	{
		Id = Guid.NewGuid();
		CreationDate = DateTimeOffset.Now.ToLocalTime();
	}

	[JsonConstructor]
	public IntegrationEvent(Guid id, DateTimeOffset createDate)
	{
		Id = id;
		CreationDate = createDate;
	}
}
