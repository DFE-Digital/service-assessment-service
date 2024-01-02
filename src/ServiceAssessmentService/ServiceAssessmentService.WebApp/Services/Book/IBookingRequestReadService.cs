using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public interface IBookingRequestReadService
{
    Task<IEnumerable<IncompleteBookingRequest>> GetAllAssessments();
    Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id);
    Task<List<DateOnly>> AvailableReviewDates(BookingRequestId id);
}
