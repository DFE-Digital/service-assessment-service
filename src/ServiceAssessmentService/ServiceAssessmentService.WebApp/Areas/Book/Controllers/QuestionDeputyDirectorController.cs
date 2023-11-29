using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;
using System.Net;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Lookups;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{

    [HttpGet]
    public async Task<IActionResult> DeputyDirector(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }


        var model = new DeputyDirectorViewModel()
        {
            RequestId = bookingRequestId,
            DeputyDirectorName = bookingRequest.DeputyDirector?.Name,
            DeputyDirectorEmail = bookingRequest.DeputyDirector?.Email,
            NameErrors = new List<string>(),
            EmailErrors = new List<string>(),
        };

        return View("DeputyDirector", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeputyDirector(Guid id, [FromForm] DeputyDirectorDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateDeputyDirector(bookingRequestId, dto.Name, dto.Email);
        if (bookingRequestChangeResult.HasErrors)
        {
            var model = new DeputyDirectorViewModel()
            {
                RequestId = bookingRequestId,

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                DeputyDirectorName = dto.Name,
                DeputyDirectorEmail = dto.Email,

                // FIXME: Response doesn't currently differentiate which field the error message is associated with, therefore all errors are displayed for both fields:
                NameErrors = bookingRequestChangeResult.Errors.Select(e => e.Message),
                EmailErrors = bookingRequestChangeResult.Errors.Select(e => e.Message),
            };

            return View("DeputyDirector", model);
        }

        return RedirectToAction(nameof(SeniorResponsibleOfficer), new { id = bookingRequestId });
    }
}


public sealed class DeputyDirectorViewModel
{
    public BookingRequestId RequestId { get; init; }

    public string? DeputyDirectorName { get; set; }
    public string? DeputyDirectorEmail { get; set; }


    public string FormElementNameDeputyDirectorName => DeputyDirectorDto.FormNameDeputyDirectorName;
    public string FormElementNameDeputyDirectorEmail => DeputyDirectorDto.FormNameDeputyDirectorEmail;

    public IEnumerable<string> NameErrors { get; set; } = new List<string>();
    public IEnumerable<string> EmailErrors { get; set; } = new List<string>();
    public IEnumerable<string> AllErrors => NameErrors.Concat(EmailErrors);
}

public sealed class DeputyDirectorDto
{
    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormNameDeputyDirectorName = "deputyDirector_name";

    // Validations (e.g., length, allowable characters, etc.) are implemented via API
    [FromForm(Name = FormNameDeputyDirectorName)]
    public string Name { get; init; } = null!;

    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormNameDeputyDirectorEmail = "deputyDirector_email";

    // Validations (e.g., length, allowable characters, etc.) are implemented via API
    [FromForm(Name = FormNameDeputyDirectorEmail)]
    public string Email { get; init; } = null!;
}
