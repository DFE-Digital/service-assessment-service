using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;
using System.Diagnostics.CodeAnalysis;
using ServiceAssessmentService.WebApp.Services.Book;

namespace ServiceAssessmentService.WebApp.Test.Book;

/// <summary>
/// Stub fake booking request read service.
/// All implementations of interface methods throw NotImplementedException,
/// and are virtual so that they may be overridden as required.
/// </summary>
[ExcludeFromCodeCoverage]
public class FakeNotImplementedBookingRequestReadService : IBookingRequestReadService
{
    public Task<IEnumerable<IncompleteBookingRequest>> GetAllAssessments()
    {
        throw new NotImplementedException();
    }

    public virtual Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
    {
        throw new NotImplementedException();
    }

    public Task<List<DateOnly>> AvailableReviewDates(BookingRequestId id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IEnumerable<Phase>> GetPhases()
    {
        throw new NotImplementedException();
    }
}