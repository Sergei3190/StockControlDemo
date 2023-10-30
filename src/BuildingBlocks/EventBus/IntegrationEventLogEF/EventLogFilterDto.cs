namespace IntegrationEventLogEF;

/// <summary>
/// Фильтр журнала интеграционного события
/// </summary>
public class EventLogFilterDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int MaxTimeSent { get; set; }
    public EventStateEnum[]? States { get; set; }
    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;

    public EventLogFilterDto()
        : this(1, 20, 0, null)
    {

    }

    public EventLogFilterDto(int page, int pageSize, int maxTimeSent, EventStateEnum[]? states = null)
    {
        Page = page <= 0 ? 1 : page;
        PageSize = pageSize;
        MaxTimeSent = maxTimeSent;
        States = states;
    }
}