using System;
using System.Threading.Tasks;
using ServiceAssessmentService.WebApp.Interfaces;

namespace ServiceAssessmentService.WebApp.Services
{
    public class MagicLinkService : IMagicLinkService
    {
        public async Task<string> GenerateMagicLinkAsync(string userId)
        {
            // Generate a random token for the magic link
            string randomToken = GenerateRandomToken();

            // Construct the magic link using the generated token and other parameters
            string magicLink = $"https://example.com/magiclink?token={randomToken}&userId={userId}";

            // For now, we'll just return the magic link without sending an email
            return magicLink;
        }

        public async Task<bool> VerifyMagicLinkAsync(string magicLink)
        {
            // Here you would implement your logic to verify the magic link.
            // For now, let's assume any magic link with a valid format is considered valid.
            // You might want to validate the token, check its expiration, or verify the userId.

            // For demonstration purposes, we'll just return true.
            return true;
        }

        // This method generates a random token (for demonstration purposes)
        private string GenerateRandomToken()
        {
            // Generate a random GUID and convert it to a string
            string token = Guid.NewGuid().ToString();

            // You might want to customize the format or length of the token according to your requirements

            return token;
        }
    }
}
