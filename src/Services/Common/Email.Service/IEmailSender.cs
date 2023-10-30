namespace Email.Service;

/// <summary>
/// Контракт отправителя сообщения
/// </summary>
public interface IEmailSender
{
    Task SendEmailAsync(Message message);
}
