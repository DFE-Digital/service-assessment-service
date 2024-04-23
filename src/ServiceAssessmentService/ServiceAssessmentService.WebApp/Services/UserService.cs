using ServiceAssessmentService.WebApp.Interfaces;
using System.Threading.Tasks;

namespace ServiceAssessmentService.WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMagicLinkService _magicLinkService;

        public UserService(IMagicLinkService magicLinkService)
        {
            _magicLinkService = magicLinkService;
        }

        public async Task<string> RegisterUserAsync(string email, string name)
        {
            // Here you would typically save the user to your database and generate a unique user ID
            // For simplicity, let's assume we generate a unique user ID using the email for now
            string userId = email.ToLowerInvariant(); // This could be a more sophisticated method

            // Generate a magic link for the user
            string magicLink = await _magicLinkService.GenerateMagicLinkAsync(userId);

            // Return the magic link
            return magicLink;
        }

        public async Task<bool> VerifyMagicLinkAsync(string userId, string magicLink)
        {
            // Here you would typically verify the magic link against your database
            // For simplicity, let's just return true for now
            return true;
        }
    }
}
