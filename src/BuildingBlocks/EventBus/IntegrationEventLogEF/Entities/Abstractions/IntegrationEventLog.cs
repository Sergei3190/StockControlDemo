using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationEventLogEF.Entities.Abstractions;

/// <summary>
/// Абстракция интеграционного события
/// </summary>
public abstract class IntegrationEventLog
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Идентфикатор публикуемого/обрабатываемого интеграционного события
    /// </summary>
    public Guid EventId { get; protected set; }

    /// <summary>
    /// Время создания публикуемого/обрабатываемого интеграционного события
    /// </summary>
    public DateTimeOffset CreationTime { get; protected set; }

    /// <summary>
    /// Имя интеграционного события
    /// </summary>
    public string EventTypeName { get; protected set; }

    /// <summary>
    /// Транзакция локальной бд, которая объединяет два контекста : доменных моделей микросервиса отправителя/получателя и интеграционных событий
    /// </summary>
    public Guid TransactionId { get; protected set; }
}

public static class IntegrationEventLogConfiguraton
{
    public static void MapIntegrationEventLogEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : IntegrationEventLog
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.EventId).HasColumnName("event_id");
        builder.Property(x => x.CreationTime).HasColumnName("creation_time");
        builder.Property(x => x.EventTypeName).HasColumnName("event_type_name");
        builder.Property(x => x.TransactionId).HasColumnName("transaction_id");

        builder.HasIndex(x => x.EventId).IsUnique(); //  уникальное значение для каждой записи
        builder.HasIndex(x => x.EventTypeName);
    }
}