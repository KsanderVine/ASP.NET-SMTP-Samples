using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Services;

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IEmailSender _emailSender;

        public EmailController(
            ILogger<EmailController> logger,
            IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<ActionResult> Send ([FromBody] EmailOptions email)
        {
            _logger.LogInformation("Trying send email...");

            await _emailSender.SendEmailAsync(new EmailOptions
            {
                To = email.To,
                Subject = email.Subject,
                Body = email.Body
            });

            return Ok();
        }
    }
}
