using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Options;

using MimeKit;
using MimeKit.Text;

namespace CleanArchitecuture.Net7.EmailProvider;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public SmtpEmailService(IOptions<SmtpSettings> appSettings)
    {
        _smtpSettings = appSettings.Value;
    }

    public async Task SendAsync(string to, string subject, string html, string from = null)
    {
        // create message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from ?? _smtpSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpSettings.SmtpHost, _smtpSettings.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpSettings.SmtpUser, _smtpSettings.SmtpPass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}