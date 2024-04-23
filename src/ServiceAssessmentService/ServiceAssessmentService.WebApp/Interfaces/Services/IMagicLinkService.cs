using System.Threading.Tasks;

namespace ServiceAssessmentService.WebApp.Interfaces
{
    public interface IMagicLinkService
    {
        Task<string> GenerateMagicLinkAsync(string userid);
        Task<bool> VerifyMagicLinkAsync(string magicLink);
    }
}
