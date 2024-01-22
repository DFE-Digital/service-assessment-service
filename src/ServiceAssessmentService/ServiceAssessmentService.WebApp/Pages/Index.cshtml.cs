using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceAssessmentService.WebApp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        if (User?.Identity?.IsAuthenticated ?? false)
        {
            // If logged in, redirect to dashboard
            return RedirectToPage("/Dashboard");
        }

        // Otherwise, redirect to the public index page
        return RedirectToPage("/Public/Index");

    }
}
