using System.Threading.Tasks;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Interfaces
{
    public interface ICreateUserService
    {
        Task<string> RegisterUserAsync(SignUpModel person);
        Task<bool> VerifyMagicLinkAsync(User person, string magicLink);
        Task<bool> SendEmailAsync(string email, string userId);

    }
}
