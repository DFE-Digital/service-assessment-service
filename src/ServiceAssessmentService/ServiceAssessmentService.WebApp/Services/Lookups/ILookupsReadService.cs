using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Lookups;

public interface ILookupsReadService
{
    Task<IEnumerable<Phase>> GetPhases();
    Task<IEnumerable<AssessmentType>> GetAssessmentTypes();
}