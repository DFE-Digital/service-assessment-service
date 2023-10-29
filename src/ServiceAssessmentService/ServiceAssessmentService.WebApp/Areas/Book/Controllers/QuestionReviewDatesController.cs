using Humanizer;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{
    
    
    [HttpGet]
    public async Task<IActionResult> ReviewDates(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var availableReviewDates = await _bookingRequestReadService.AvailableReviewDates(bookingRequestId);
        var model = new ReviewDatesViewModel
        {
            RequestId = bookingRequestId,

            EndDate = bookingRequest.EndDate,
            SelectedReviewDates = bookingRequest.ReviewDates,
            AvailableReviewDates = availableReviewDates,

            ReviewDateErrors = new List<string>(),
        };

        return View(model);
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReviewDates(Guid id, [FromForm] ReviewDateDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        
        var submittedDates = (dto.Value is null)
            ? new List<DateOnly>()
            : dto.Value.Select(DateOnly.Parse).ToList();
        
        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateReviewDates(bookingRequestId, submittedDates);

        if (bookingRequestChangeResult.HasErrors)
        {
            var request = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
            var availableReviewDates = await _bookingRequestReadService.AvailableReviewDates(bookingRequestId);

            var model = new ReviewDatesViewModel
            {
                RequestId = bookingRequestId,
                EndDate = request.EndDate,

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                SelectedReviewDates = submittedDates,
                AvailableReviewDates = availableReviewDates,

                ReviewDateErrors = bookingRequestChangeResult.Errors.Select(e => e.Message).ToList(),
            };

            return View(model);
        }

        return RedirectToAction(nameof(Portfolio), new { id = bookingRequestId });
    }
}

public sealed class ReviewDatesViewModel
{
    public BookingRequestId RequestId { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<DateOnly> SelectedReviewDates { get; set; }
    public List<DateOnly> AvailableReviewDates { get; set; }
    public List<string> ReviewDateErrors { get; set; }
    
    public bool IsReviewDateWithinNextFiveWeeks => EndDate is not null && EndDate.Value <= DateOnly.FromDateTime(DateTime.Today.AddDays(5 * 7));
}


public sealed class ReviewDateDto
{
    public const string ReviewDateFormName = "review-date";

    [FromForm(Name = ReviewDateFormName)]
    public List<string> Value { get; init; } = null;
}

