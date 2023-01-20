using System.ComponentModel.DataAnnotations;

namespace SampleApp.Models
{
    public record EmailAddress([EmailAddress]string Email, string? FullName = null);
}
