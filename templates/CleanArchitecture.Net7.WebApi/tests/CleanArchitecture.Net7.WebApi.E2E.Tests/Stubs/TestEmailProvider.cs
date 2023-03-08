using CleanArchitecuture.Net7.EmailProvider;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests
{
    public class TestEmailProvider : IEmailService
    {
        public Task SendAsync(string to, string subject, string html, string from = null)
        {
            return Task.CompletedTask;
        }
    }
}
