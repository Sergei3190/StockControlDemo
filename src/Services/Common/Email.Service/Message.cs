using Microsoft.AspNetCore.Http;

using MimeKit;

namespace Email.Service;

/// <summary>
/// Отправляемое сообщение
/// </summary>
public class Message
{
    /// <summary>
    /// Адреса получателей
    /// </summary>
    public List<MailboxAddress> To { get; set; }

    /// <summary>
    /// Тема письма
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Содержимое письма
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Вложенные файлы к нашему сообщению
    /// </summary>
    public IFormFileCollection Attachments { get; set; }

    public Message(IEnumerable<(string name, string address)> to, string subject, string content, IFormFileCollection attachments)
    {
        To = new List<MailboxAddress>();

        To.AddRange(to.Select(x => new MailboxAddress(x.name, x.address)));
        Subject = subject;
        Content = content;
        Attachments = attachments;
    }
}
