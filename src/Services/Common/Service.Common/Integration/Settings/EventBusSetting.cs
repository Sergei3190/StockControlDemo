namespace Service.Common.Integration.Settings;

/// <summary>
/// Настройка шины сообщений
/// </summary>
public class EventBusSetting
{
    /// <summary>
    /// Включена шина или нет
    /// </summary>
    public bool Enabled { get; set; }
    public string Connect { get; set; }
    public RabbitMQBrokerConfig Default { get; set; }
    public RabbitMQDlxBrokerConfig DeadLetter { get; set; }
    public RabbitMQAccessConfig BusAccess { get; set; }
}