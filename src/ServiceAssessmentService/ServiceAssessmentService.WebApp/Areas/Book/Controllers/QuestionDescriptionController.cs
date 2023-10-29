using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Description(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var viewModel = new DescriptionViewModel
        {
            RequestId = bookingRequestId,
            Phase = bookingRequest.PhaseConcluding,
            AssessmentType = bookingRequest.AssessmentType,
            Description = bookingRequest.Description ?? string.Empty,
            Errors = new List<string>(),
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Description(Guid id, [FromForm] DescriptionDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateDescription(bookingRequestId, dto.Value);

        if (bookingRequestChangeResult.HasErrors)
        {
            // TODO: Include this in the change result output...?
            var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
            if (bookingRequest is null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound($"Booking request with ID {bookingRequestId} not found");
            }

            var viewModel = new DescriptionViewModel
            {
                RequestId = bookingRequestId,
                Phase = bookingRequest.PhaseConcluding,
                AssessmentType = bookingRequest.AssessmentType,
                Errors = bookingRequestChangeResult.Errors.Select(e => e.Message),

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                Description = dto.Value,
            };

            return View(viewModel);
        }

        return RedirectToAction(nameof(ProjectCode), new { id = bookingRequestId });
    }
}

public sealed class DescriptionViewModel
{
    public BookingRequestId RequestId { get; init; }
    public Phase? Phase { get; set; }
    public AssessmentType? AssessmentType { get; set; }

    public string Description { get; init; } = null!;

    public int MaxLength => DescriptionDto.MaxLength;

    public string FormElementName => DescriptionDto.FormName;


    public IEnumerable<string> Errors { get; set; } = new List<string>();
}

public sealed class DescriptionDto
{
    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormName = "service-description";
    public const int MaxLength = 500;

    [FromForm(Name = FormName)]
    public string Value { get; init; } = null!;
}
