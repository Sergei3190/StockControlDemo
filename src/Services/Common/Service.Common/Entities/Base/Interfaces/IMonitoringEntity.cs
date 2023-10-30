namespace Service.Common.Entities.Base.Interfaces;

public interface IMonitoringEntity : IEntity
{
    /// <summary>
    /// Дата создания записи
    /// </summary>
    DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Идентификатор пользователя создавшего запись
    /// </summary>
    Guid? CreatedBy { get; set; }

    /// <summary>
    /// Дата последнего редактирования записи
    /// </summary>
    DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Идентификатор пользователя в последний раз отредактировавшего запись
    /// </summary>
    Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Дата установки отметки об удалении записи
    /// </summary>
    DateTimeOffset? DeletedDate { get; set; }

    /// <summary>
    /// Идентификатор пользователя установившего отметку об удалении записи
    /// </summary>
    Guid? DeletedBy { get; set; }
}