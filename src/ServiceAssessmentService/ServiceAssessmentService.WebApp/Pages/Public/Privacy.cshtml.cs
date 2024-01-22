using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceAssessmentService.WebApp.Pages.Public;

[AllowAnonymous]
public class PrivacyModel : PageModel
{
    public void OnGet()
    {
    }
}
