using ServiceAssessmentService.WebApp.Interfaces;

using ServiceAssessmentService.WebApp.Models;
using System.Threading.Tasks;
using Notify.Interfaces;

namespace ServiceAssessmentService.WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMagicLinkService _magicLinkService;
        private readonly IEmailService _emailService;

        public UserService(IMagicLinkService magicLinkService, IEmailService notificationService)
        {
            _magicLinkService = magicLinkService;
            _emailService = notificationService;
        }

        public async Task<string> RegisterUserAsync(UserModel userModel)
        {
            // Here you would typically save the user to your database and generate a unique user ID
            // For simplicity, let's assume we generate a unique user ID using the email for now
            string userId = userModel.Email.ToLowerInvariant(); // This could be a more sophisticated method

            // Generate a magic link for the user
            string magicLink = await _magicLinkService.GenerateMagicLinkAsync(userId);

            // Return the magic link
            return magicLink;
        }

        public Task<bool> VerifyMagicLinkAsync(UserModel user, string magicLink)
        {
            return _magicLinkService.VerifyMagicLinkAsync(magicLink);
        }

       public async Task<bool> SendEmailAsync(string email, string userId)
        {
            try
            {
                // Generate magic link
                var magicLink = await _magicLinkService.GenerateMagicLinkAsync(userId);

                // Prepare personalisation data
                var personalisation = new Dictionary<string, dynamic>
                {
                    { "magicLink", magicLink }
                };

                // Fire and forget: Send email
                _ = _emailService.SendEmailAsync(email, "7ef7dbfe-6c94-4f13-8295-27b3e9ab4bba", personalisation);

                // Return true to indicate that the email sending process has started
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error sending email: {ex.Message}");

                // Return false to indicate that an error occurred during the email sending process
                return false;
            }
        }

    }
}
