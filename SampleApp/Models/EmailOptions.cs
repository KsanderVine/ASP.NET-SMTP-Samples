using SampleApp.Options;

namespace SampleApp.Models
{
    public class EmailOptions
    {
        public EmailAddress[] To { get; set; } = Array.Empty<EmailAddress>();

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
