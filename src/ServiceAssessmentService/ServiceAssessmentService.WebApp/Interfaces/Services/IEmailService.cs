using System.Threading.Tasks;
using System.Collections.Generic;

namespace ServiceAssessmentService.WebApp.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailAddress, string templateId, Dictionary<string, dynamic> personalisation, string reference = null, string emailReplyToId = null);
    }
}
