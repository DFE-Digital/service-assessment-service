using System.Threading.Tasks;

namespace ServiceAssessmentService.WebApp.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(string email, string name);
        Task<bool> VerifyMagicLinkAsync(string userId, string magicLink);
    }
}
