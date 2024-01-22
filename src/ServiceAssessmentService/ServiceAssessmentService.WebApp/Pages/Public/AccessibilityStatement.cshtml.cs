using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceAssessmentService.WebApp.Pages.Public;

[AllowAnonymous]
public class AccessibilityStatementModel : PageModel
{
    public void OnGet()
    {
    }
}
