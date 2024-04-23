using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Notify.Client;

namespace ServiceAssessmentService.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationClient _client;

        public NotificationController(IConfiguration configuration)
        {
            string apiKey = configuration["GovUKNotify:ApiKey"];
            _client = new NotificationClient(apiKey);
        }
    }
}
