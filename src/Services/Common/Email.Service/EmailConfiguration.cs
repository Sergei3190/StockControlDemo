namespace Email.Service;

/// <summary>
/// Конфигурация сервиса отправки писем
/// </summary>
public class EmailConfiguration
{
    /// <summary>
    /// Почта отправителя сообщений
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Сервер Smpt для отправки электронной почты
    /// </summary>
    public string SmtpServer { get; set; }

    /// <summary>
    /// Порт, по которому будут отправляться сообщения
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Имя почты отправителя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль почты отправителя
    /// </summary>
    public string Password { get; set; }
}
