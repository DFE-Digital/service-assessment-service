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

    public Task<IEnumerable<Portfolio>> GetPortfolioOptions()
    {
        IEnumerable<Portfolio> x = new List<Portfolio>
        {
            new() { Id = "families_group", DisplayName = "Families group", IsEnabled = true, SortOrder = 10, IsInMainSection = true},
            new() { Id = "funding_group", DisplayName = "Funding group", IsEnabled = true, SortOrder = 20, IsInMainSection = true},
            new() { Id = "operations_and_infrastructure_group", DisplayName = "Operations and Infrastructure group", IsEnabled = true, SortOrder = 30, IsInMainSection = true},
            new() { Id = "regions_group", DisplayName = "Regions group", IsEnabled = true, SortOrder = 40, IsInMainSection = true},
            new() { Id = "schools_group", DisplayName = "Schools group, includes teacher services", IsEnabled = true, SortOrder = 50, IsInMainSection = true},
            new() { Id = "skills_group", DisplayName = "Skills group", IsEnabled = true, SortOrder = 60, IsInMainSection = true},
            new() { Id = "standards_and_testing_agency", DisplayName = "Standards and Testing Agency", IsEnabled = true, SortOrder = 70, IsInMainSection = false},
            new() { Id = "student_loans_company", DisplayName = "Student Loans Company", IsEnabled = true, SortOrder = 80, IsInMainSection = false},
            new() { Id = "teaching_regulation_agency", DisplayName = "Teaching Regulation Agency", IsEnabled = true, SortOrder = 90, IsInMainSection = false},
        };

        return Task.FromResult(x);
    }

}
