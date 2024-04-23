using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services;
using ServiceAssessmentService.WebApp.Interfaces;
using System.Threading.Tasks;

namespace ServiceAssessmentService.WebApp.Pages.Page_Models.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginViewModel Login { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Call your user service to handle login logic
            bool isMagicLinkSent = await _userService.SendEmailAsync(Login.Email, Login.ID);

            if (isMagicLinkSent)
            {
                // Redirect to a page indicating that the magic link has been sent
                return RedirectToPage("/MagicLinkSent");
            }
            else
            {
                // Display an error message if sending the magic link fails
                ModelState.AddModelError(string.Empty, "Failed to send magic link. Please try again.");
                return Page();
            }
        }

        private string GenerateUserID()
        {
            // Generate a unique user ID using the email for simplicity
            return Login.Email.ToLowerInvariant();
        }
    }
}
