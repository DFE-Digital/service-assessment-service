using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Pages.Me;

[AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
public class IndexModel : PageModel
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly GraphUserClient _graphUserClient;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, GraphServiceClient graphServiceClient, GraphUserClient graphUserClient)
    {
        _logger = logger;
        _graphServiceClient = graphServiceClient;
        _graphUserClient = graphUserClient;
    }

    public async Task OnGet()
    {
        var user = await _graphUserClient.GetGraphUser();
        ViewData["GraphUser"] = user;
        ViewData["GraphApiResult"] = user.DisplayName;
    }
}
