using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

using EventBus.Events;

using IntegrationEventLogEF.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationEventLogEF.Entities;

/// <summary>
/// Сущность бд, в которой будут храниться исходящие интеграционные события микросервиса/отправителя.
/// Данная таблица создана для атомарности операций с доменным объектом исходной бд и созданием интеграционного события
/// </summary>
public class ExportIntegrationEventLog : IntegrationEventLog, ICloneable
{
    private ExportIntegrationEventLog() { }

    public ExportIntegrationEventLog(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName!;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });
        State = EventStateEnum.NotPublished.ToString();
        TimesSent = 0;
        TransactionId = transactionId;
    }

    /// <summary>
    /// Состояние интеграционного события
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Содержимое передаваемого события
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Кол-во повторных публикаций данного события
    /// </summary>
    public int TimesSent { get; set; }

    /// <summary>
    /// Красткое описание ошибки в случаи статуса PublishedFailed или превышения максимального кол-ва повторных отправок
    /// </summary>
    public string? Error { get; set; }

    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.')?.Last()!;

    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }

    public ExportIntegrationEventLog DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })
            as IntegrationEvent;

        return this;
    }

    public object Clone()
    {
        return new ExportIntegrationEventLog(IntegrationEvent!, TransactionId);
    }

    public class Map : IEntityTypeConfiguration<ExportIntegrationEventLog>
    {
        public void Configure(EntityTypeBuilder<ExportIntegrationEventLog> builder)
        {
            builder.ToTable("export_integration_event_log", "logger");

            builder.MapIntegrationEventLogEntity();

            builder.Property(x => x.Content).HasColumnName("content");
            builder.Property(x => x.TimesSent).HasColumnName("times_sent");
            builder.Property(x => x.State).HasColumnName("state").HasColumnType("nvarchar(25)");
            builder.Property(x => x.Error).HasColumnName("error");

            builder.HasIndex(x => new { x.EventTypeName, x.State });
            builder.HasIndex(x => x.State);
        }
    }
}
