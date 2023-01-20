using SampleApp.Models;
using SampleApp.Options;
using System.Net;
using System.Net.Mail;

namespace SampleApp.Services
{
    public sealed class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly EmailSenderOptions _options;
        private const string SenderOptionsSectionName = "EmailSenderOptions";

        public EmailSender(
            ILogger<EmailSender> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _options = configuration
                .GetSection(SenderOptionsSectionName)
                .Get<EmailSenderOptions>();
        }

        public async Task SendEmailAsync(EmailOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options), $"Value of \"{nameof(options)}\" can not be null");

            if (options.To is null || !options.To.Any())
                throw new NullReferenceException($"Value of \"{nameof(options.To)}\" can not be null or empty");

            _logger.LogInformation("Sending email to [{to}] using sender [{sender}]", 
                string.Join(", ", options.To.Select(x => x.Email)),
                nameof(EmailSender));

            var senderOptions = _options;
            var mailMessage = ToMailMessage(options, senderOptions);

            using var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(
                senderOptions.SmtpUser.Email, 
                senderOptions.SmtpPassword);

            smtpClient.Host = senderOptions.SmtpHost;
            smtpClient.Port = senderOptions.SmtpPort;
            smtpClient.EnableSsl = senderOptions.SmtpSsl;
            
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }

        private static MailMessage ToMailMessage (EmailOptions options, EmailSenderOptions senderOptions)
        {
            MailMessage message = new();

            foreach(var to in options.To!)
                message.To.Add(new MailAddress(to.Email, to.FullName));

            message.From = new MailAddress(
                senderOptions.SmtpUser.Email, 
                senderOptions.SmtpUser.FullName);

            message.Subject = options.Subject;
            message.Body = options.Body + nameof(EmailSender);

            return message;
        }
    }
}
