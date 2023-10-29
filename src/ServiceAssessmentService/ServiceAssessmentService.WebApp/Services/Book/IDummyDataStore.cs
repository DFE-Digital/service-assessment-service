using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public interface IDummyDataStore
{
    Task<IncompleteBookingRequest> CreateNewBookingRequest(Phase? concludingPhase = null, AssessmentType? assessmentType = null);
    Task<IEnumerable<IncompleteBookingRequest>> GetAllAssessments();
    Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id);
    Task<IncompleteBookingRequest> Put(BookingRequestId id, IncompleteBookingRequest incompleteBookingRequest);
}
