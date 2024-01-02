using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public class ApiBookingRequestReadService : IBookingRequestReadService
{
    public Task<IEnumerable<IncompleteBookingRequest>> GetAllAssessments()
    {
        throw new NotImplementedException();
    }

    public Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
    {
        throw new NotImplementedException();
    }

    public Task<List<DateOnly>> AvailableReviewDates(BookingRequestId id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Phase>> GetPhases()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AssessmentType>> GetAssessmentTypes()
    {
        throw new NotImplementedException();
    }
}
