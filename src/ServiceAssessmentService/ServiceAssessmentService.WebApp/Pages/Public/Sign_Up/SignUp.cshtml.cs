using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.WebApp.Interfaces;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services;
using System.Threading.Tasks;

namespace ServiceAssessmentService.WebApp.Pages.Page_Models.Account
{
    public class RegisterUserModel : PageModel
    {
        private readonly ICreateUserService _createUserService;

        public RegisterUserModel(ICreateUserService createUserService)
        {
            _createUserService = createUserService;
        }

        [BindProperty]
        public SignUpModel RegisterUser { get; set; }

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

            // Call the CreateUserService to register the user
            var response = await _createUserService.RegisterUserAsync(RegisterUser);

            if (response != null)
            {
                // Redirect to a page indicating that the magic link has been sent
                return RedirectToPage("/MagicLinkSent");
            }
            else
            {
                // Display an error message if registration fails
                ModelState.AddModelError(string.Empty, "Failed to register user. Please try again.");
                return Page();
            }
        }
    }
}
