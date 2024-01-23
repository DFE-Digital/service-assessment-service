using Microsoft.Graph;
using ServiceAssessmentService.WebApp.Pages.Me;

namespace ServiceAssessmentService.WebApp.Models;

public class GraphUserClient
{

    private readonly GraphServiceClient _graphServiceClient;
    private readonly ILogger<GraphUserClient> _logger;

    private Microsoft.Graph.User? _graphUser = null;

    public GraphUserClient(GraphServiceClient graphServiceClient, ILogger<GraphUserClient> logger)
    {
        _graphServiceClient = graphServiceClient;
        _logger = logger;
    }

    public async Task<Microsoft.Graph.User> GetGraphUser()
    {
        if (_graphUser == null)
        {
            _graphUser = await _graphServiceClient.Me.Request().GetAsync();
        }

        return _graphUser;
    }

    public async Task<string> GetGraphUserDisplayName()
    {
        var graphUser = await GetGraphUser();
        return graphUser.DisplayName;
    }

    public async Task<string> GetGraphUserGivenName()
    {
        var graphUser = await GetGraphUser();
        return graphUser.GivenName;
    }

    public async Task<string> GetGraphUserSurname()
    {
        var graphUser = await GetGraphUser();
        return graphUser.Surname;
    }

    public async Task<string> GetGraphUserMail()
    {
        var graphUser = await GetGraphUser();
        return graphUser.Mail;
    }

}
