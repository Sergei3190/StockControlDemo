using EventBus.Events;

using IntegrationEventLogEF.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationEventLogEF.Entities;

/// <summary>
/// Сущность бд, в которой будет храниться краткая информация об УСПЕШНО обработанных интеграционных событиях.
/// Данная сущность будет помогать сохранять идемпотентность и обновление доменных данных микросервиса/подписчика актуальной информацией + 
/// обеспечивает атомарность операции создания/обновления доменной модели и сохранения результата обработки входящего интеграционного события
/// </summary>
public class ImportSuccessIntegrationEventLog : IntegrationEventLog
{
    private ImportSuccessIntegrationEventLog() { }

    public ImportSuccessIntegrationEventLog(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName!;
        TransactionId = transactionId;
        ProcessingEndDate = DateTimeOffset.Now.ToLocalTime();
    }

    /// <summary>
    /// Дата окончания обоработки интеграционного события, неважно успешной или неуспешной
    /// </summary>
    public DateTimeOffset? ProcessingEndDate { get; set; }

    public class Map : IEntityTypeConfiguration<ImportSuccessIntegrationEventLog>
    {
        public void Configure(EntityTypeBuilder<ImportSuccessIntegrationEventLog> builder)
        {
            builder.ToTable("import_integration_event_log", "logger");

            builder.MapIntegrationEventLogEntity();

            builder.Property(x => x.ProcessingEndDate).HasColumnName("processing_end_date");
        }
    }
}
