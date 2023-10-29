using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Areas.Book.Views.Shared;
using ServiceAssessmentService.WebApp.Core;
using System.Net;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{
    private static TextQuestionModel NewScaffoldedServiceNameModel(bool isDiscovery)
    {
        return new TextQuestionModel
        {
            FormController = "BookingRequest",
            FormAction = nameof(Name),
            InputName = AssessmentNameDto.FormName,
            QuestionText = (isDiscovery) ? "What is the name of your discovery?" : "What is the name of your service?",
            QuestionHintText = (isDiscovery) ? "This can be changed in the future if you develop a service." : string.Empty,
        };
    }


    [HttpGet]
    public async Task<IActionResult> Name(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var isDiscovery = "discovery".Equals(bookingRequest.PhaseConcluding?.Id, StringComparison.Ordinal);
        var model = NewScaffoldedServiceNameModel(isDiscovery);
        model.BookingRequestId = bookingRequestId;
        model.Value = bookingRequest.Name ?? string.Empty;
        model.Errors = new List<string>();

        return View("TextQuestion", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Name(Guid id, [FromForm] AssessmentNameDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateRequestName(bookingRequestId, dto.Value);

        if (bookingRequestChangeResult.HasErrors)
        {
            // TODO: Include this in the change result output...?
            var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
            if (bookingRequest is null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound($"Booking request with ID {bookingRequestId} not found");
            }

            var isDiscovery = "discovery".Equals(bookingRequest.PhaseConcluding?.Id, StringComparison.Ordinal);
            var model = NewScaffoldedServiceNameModel(isDiscovery);
            model.BookingRequestId = bookingRequestId;
            model.Value = dto.Value;
            model.Errors = bookingRequestChangeResult.Errors.Select(e => e.Message);


            // HTTP 422 - Unprocessable Entity, to indicate the submitted value is not acceptable/not valid
            Response.StatusCode = 422;

            return View("TextQuestion", model);
        }

        return RedirectToAction(nameof(Description), new { id = bookingRequestId });
    }
}

public sealed class AssessmentNameDto
{
    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormName = "service-name";

    // Validations (e.g., length, allowable characters, etc.) are implemented via API
    [FromForm(Name = FormName)]
    public string Value { get; init; } = null!;
}
