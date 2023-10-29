using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public class ApiBookingRequestWriteService : IBookingRequestWriteService
{

    private readonly ILogger<ApiBookingRequestWriteService> _logger;

    public ApiBookingRequestWriteService(ILogger<ApiBookingRequestWriteService> logger)
    {
        _logger = logger;
    }

    public Task<IncompleteBookingRequest> CreateRequestAsync(Phase phaseConcluding, AssessmentType assessmentType)
    {
        _logger.LogWarning("Called {WriteServiceActionName} from {WriteServiceImplementationName}, which is not yet implemented", nameof(CreateRequestAsync), nameof(ApiBookingRequestWriteService));
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateRequestName(BookingRequestId id, string proposedName)
    {
        _logger.LogWarning("Called {WriteServiceActionName} from {WriteServiceImplementationName}, which is not yet implemented", nameof(UpdateRequestName), nameof(ApiBookingRequestWriteService));
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateDescription(BookingRequestId id, string proposedDescription)
    {
        _logger.LogWarning("Called {WriteServiceActionName} from {WriteServiceImplementationName}, which is not yet implemented", nameof(UpdateDescription), nameof(ApiBookingRequestWriteService));
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateProjectCode(BookingRequestId id, bool? isProjectCodeKnown,
        string proposedProjectCode)
    {
        _logger.LogWarning("Called {WriteServiceActionName} from {WriteServiceImplementationName}, which is not yet implemented", nameof(UpdateProjectCode), nameof(ApiBookingRequestWriteService));
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateStartDate(BookingRequestId id, string? proposedYear, string? proposedMonth, string? proposedDayOfMonth)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateEndDate(BookingRequestId id, bool? isEndDateKnown, string? proposedYear, string? proposedMonth,
        string? proposedDayOfMonth)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateReviewDates(BookingRequestId id, List<DateOnly> proposedReviewDates)
    {
        throw new NotImplementedException();
    }

}
