using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using SampleApp.Models;
using SampleApp.Options;
using System.Net;
using System.Net.Mail;

namespace SampleApp.Services
{
    public sealed class FluentEmailSender : IEmailSender
    {
        private readonly ILogger<FluentEmailSender> _logger;
        private readonly EmailSenderOptions _options;
        private const string SenderOptionsSectionName = "EmailSenderOptions";

        public FluentEmailSender(
            ILogger<FluentEmailSender> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _options = configuration
                .GetSection(SenderOptionsSectionName)
                .Get<EmailSenderOptions>();

            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(
                _options.SmtpUser.Email,
                _options.SmtpPassword);

            smtpClient.Host = _options.SmtpHost;
            smtpClient.Port = _options.SmtpPort;
            smtpClient.EnableSsl = _options.SmtpSsl;

            Email.DefaultSender = new SmtpSender(smtpClient);
        }

        public async Task SendEmailAsync(EmailOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options), $"Value of \"{nameof(options)}\" can not be null");

            if (options.To is null || !options.To.Any())
                throw new NullReferenceException($"Value of \"{nameof(options.To)}\" can not be null or empty");

            _logger.LogInformation("Sending email to [{to}] using sender [{sender}]",
                string.Join(", ", options.To.Select(x => x.Email)),
                nameof(FluentEmailSender));

            var senderOptions = _options;

            var email = await Email
                .From(senderOptions.SmtpUser.Email, senderOptions.SmtpUser.FullName)
                .To(options.To.Select(x => new Address(x.Email, x.FullName)))
                .Subject(options.Subject)
                .Body(options.Body)
                .SendAsync(cancellationToken);

            if (email.Successful)
            {
                _logger.LogInformation("Email delivery status: Successful!");
            }
            else
            {
                _logger.LogError("Email delivery status: Error...");
            }
        }
    }
}
