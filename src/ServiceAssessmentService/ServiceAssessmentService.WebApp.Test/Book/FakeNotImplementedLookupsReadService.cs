using ServiceAssessmentService.WebApp.Models;
using System.Diagnostics.CodeAnalysis;
using ServiceAssessmentService.WebApp.Services.Lookups;

namespace ServiceAssessmentService.WebApp.Test.Book;

/// <summary>
/// Stub fake booking request read service.
/// All implementations of interface methods throw NotImplementedException,
/// and are virtual so that they may be overridden as required.
/// </summary>
[ExcludeFromCodeCoverage]
public class FakeNotImplementedLookupsReadService : ILookupsReadService
{
    public virtual Task<IEnumerable<Phase>> GetPhases()
    {
        throw new NotImplementedException();
    }

    public virtual Task<IEnumerable<AssessmentType>> GetAssessmentTypes()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Portfolio>> GetPortfolioOptions()
    {
        throw new NotImplementedException();
    }
}
