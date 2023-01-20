using SampleApp.Models;

namespace SampleApp.Options
{
    public sealed class EmailSenderOptions
    {
        public const string Section = nameof(EmailSenderOptions);

        public EmailAddress SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool SmtpSsl { get; set; }

        public EmailSenderOptions()
        {
            SmtpUser = new EmailAddress(string.Empty);
            SmtpPassword = string.Empty;
            SmtpHost = string.Empty;
            SmtpPort = 587;
            SmtpSsl = true;
        }

        public EmailSenderOptions(
            EmailAddress smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort = 587,
            bool smtpSsl = true)
        {
            SmtpUser = smtpUser;
            SmtpPassword = smtpPassword;
            SmtpHost = smtpHost;
            SmtpPort = smtpPort;
            SmtpSsl = smtpSsl;
        }
    }
}
