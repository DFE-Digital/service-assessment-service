using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;
using System.Diagnostics.CodeAnalysis;
using ServiceAssessmentService.WebApp.Services.Book;

namespace ServiceAssessmentService.WebApp.Test.Book;

/// <summary>
/// Stub fake booking request write service.
/// All implementations of interface methods throw NotImplementedException,
/// and are virtual so that they may be overridden as required.
/// </summary>
[ExcludeFromCodeCoverage]
public class FakeNotImplementedBookingRequestWriteService : IBookingRequestWriteService
{

    public virtual Task<IncompleteBookingRequest> CreateRequestAsync(Phase phaseConcluding, AssessmentType assessmentType)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ChangeRequestModel> UpdateRequestName(BookingRequestId id, string proposedName)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ChangeRequestModel> UpdateDescription(BookingRequestId id, string proposedDescription)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ChangeRequestModel> UpdateProjectCode(BookingRequestId id, bool? isProjectCodeKnown,
        string proposedProjectCode)
    {
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

    public Task<ChangeRequestModel> UpdatePortfolio(BookingRequestId bookingRequestId, string dtoValue)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateDeputyDirector(BookingRequestId bookingRequestId, string proposedDeputyDirectorName,
        string proposedDeputyDirectorEmail)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequestModel> UpdateReviewDate(BookingRequestId bookingRequestId, DateOnly? proposedReviewDate)
    {
        throw new NotImplementedException();
    }
}
