using SampleApp.Models;

namespace SampleApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailOptions options, CancellationToken cancellationToken = default);
    }
}
