using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public interface IBookingRequestWriteService
{
    Task<IncompleteBookingRequest> CreateRequestAsync(Phase phaseConcluding, AssessmentType assessmentType);

    // TODO: Consider having different response models, specific to each field (as opposed to a single generic `ChangeRequestModel` model)
    Task<ChangeRequestModel> UpdateRequestName(BookingRequestId id, string proposedName);
    Task<ChangeRequestModel> UpdateDescription(BookingRequestId id, string proposedDescription);
    Task<ChangeRequestModel> UpdateProjectCode(BookingRequestId id, bool? isProjectCodeKnown, string proposedProjectCode);
    Task<ChangeRequestModel> UpdateStartDate(BookingRequestId id, string? proposedYear, string? proposedMonth, string? proposedDayOfMonth);
    Task<ChangeRequestModel> UpdateEndDate(BookingRequestId id, bool? isEndDateKnown, string? proposedYear, string? proposedMonth, string? proposedDayOfMonth);
    Task<ChangeRequestModel> UpdateReviewDates(BookingRequestId id, List<DateOnly> proposedReviewDates);
    Task<ChangeRequestModel> UpdatePortfolio(BookingRequestId id, string dtoValue);
}
