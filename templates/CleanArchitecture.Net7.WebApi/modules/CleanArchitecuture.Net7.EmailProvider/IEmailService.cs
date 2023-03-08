namespace CleanArchitecuture.Net7.EmailProvider;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string html, string from = null);
}
