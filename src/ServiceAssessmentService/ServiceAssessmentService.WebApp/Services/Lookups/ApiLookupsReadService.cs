using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Lookups;

public class ApiLookupsReadService : ILookupsReadService
{
    public Task<IEnumerable<Phase>> GetPhases()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AssessmentType>> GetAssessmentTypes()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Portfolio>> GetPortfolioOptions()
    {
        throw new NotImplementedException();
    }
}
