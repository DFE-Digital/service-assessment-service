using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Lookups;

public class DummyLookupsReadService : ILookupsReadService
{

    public Task<IEnumerable<Phase>> GetPhases()
    {
        IEnumerable<Phase> x = new List<Phase>
        {
            new() { Id = "discovery", DisplayNameLowerCase = "discovery", IsEnabled = true, SortOrder = 10},
            new() { Id = "alpha", DisplayNameLowerCase = "alpha", IsEnabled = false, SortOrder = 20},
            new() { Id = "private_beta", DisplayNameLowerCase = "private beta", IsEnabled = false, SortOrder = 30},
            new() { Id = "public_beta", DisplayNameLowerCase = "public beta", IsEnabled = false, SortOrder = 40},
        };

        return Task.FromResult(x);
    }

    public Task<IEnumerable<AssessmentType>> GetAssessmentTypes()
    {
        IEnumerable<AssessmentType> x = new List<AssessmentType>
        {
            new() { Id = "peer_review", DisplayNameLowerCase = "peer review", IsEnabled = true, SortOrder = 10},
            new() { Id = "end_of_phase_assessment", DisplayNameLowerCase = "end of phase assessment", IsEnabled = false, SortOrder = 20},
            new() { Id = "mock_assessment", DisplayNameLowerCase = "mock assessment", IsEnabled = false, SortOrder = 30},
        };

        return Task.FromResult(x);
    }
}
