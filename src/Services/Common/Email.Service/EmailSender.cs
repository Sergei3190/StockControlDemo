using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;

using MimeKit;

namespace Email.Service;

/// <summary>
/// Отправитель сообщения
/// </summary>
public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(EmailConfiguration emailConfig, ILogger<EmailSender> logger)
    {
        _emailConfig = emailConfig;
        _logger = logger;
    }

    public async Task SendEmailAsync(Message message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        var mailMessage = CreateEmailMessage(message);

        await SendAsync(mailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Администратор", _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<p>{0}</p>", message.Content) };

        if (message.Attachments != null && message.Attachments.Any())
        {
            byte[] fileBytes;
            foreach (var attachment in message.Attachments)
            {
                using (var ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
				client.CheckCertificateRevocation = false;

				await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.Auto);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch(Exception ex)
            {
                _logger.LogError("Ошибка при отправке сообщения {0}",ex);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
