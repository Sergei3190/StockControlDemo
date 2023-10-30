namespace EventBusRabbitMQ.Interfaces;

/// <summary>
/// Конфигурация доступа к RabbitMQ
/// </summary>
public interface IRabbitMQAccessConfig
{
    /// <summary>
    /// Хост, на котором расположен клиент RabbitMQ
    /// </summary>
    string Host { get; init; }

    /// <summary>
    /// Имя пользователя, зарегестрированного в RabbitMQ
    /// </summary>
    string UserName { get; init; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    string Password { get; init; }

    /// <summary>
    /// Кол-во повторных попыток публикации сообщения(й)
    /// </summary>
    int RetryCount { get; init; }
}