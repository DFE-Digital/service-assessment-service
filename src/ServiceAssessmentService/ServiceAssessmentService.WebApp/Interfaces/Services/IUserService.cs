using System.Threading.Tasks;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(UserModel user);
        Task<bool> VerifyMagicLinkAsync(UserModel user, string magicLink);
        Task<bool> SendEmailAsync(string email, string userId);

    }
}
