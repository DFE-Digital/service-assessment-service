using System.Net;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{

    [HttpGet]
    public async Task<IActionResult> ProjectCode(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        string? isProjectKnown = bookingRequest.IsProjectCodeKnown switch
        {
            true => ProjectCodeDto.IsProjectCodeKnownValueYes,
            false => ProjectCodeDto.IsProjectCodeKnownValueNo,
            _ => null,
        };

        var model = new ProjectCodeViewModel
        {
            RequestId = bookingRequestId,
            IsProjectCodeKnown = isProjectKnown,
            ProjectCode = bookingRequest.ProjectCode ?? string.Empty,
            Errors = new List<string>(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProjectCode(Guid id, [FromForm] ProjectCodeDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);

        bool? isProjectKnown = dto.IsProjectCodeKnown switch
        {
            ProjectCodeDto.IsProjectCodeKnownValueYes => true,
            ProjectCodeDto.IsProjectCodeKnownValueNo => false,
            _ => null,
        };

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateProjectCode(bookingRequestId, isProjectKnown, dto.ProjectCodeValue);

        if (bookingRequestChangeResult.HasErrors)
        {
            // TODO: Include this in the change result output...?
            var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
            if (bookingRequest is null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound($"Booking request with ID {bookingRequestId} not found");
            }

            var viewModel = new ProjectCodeViewModel
            {
                RequestId = bookingRequestId,
                Errors = bookingRequestChangeResult.Errors.Select(e => e.Message),

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                IsProjectCodeKnown = dto.IsProjectCodeKnown,
                ProjectCode = dto.ProjectCodeValue,
            };

            return View(viewModel);
        }

        return RedirectToAction(nameof(StartDate), new { id = bookingRequestId });
    }


}

public sealed class ProjectCodeViewModel
{
    public BookingRequestId RequestId { get; init; }

    public string? IsProjectCodeKnown { get; init; } = null;

    public List<string> IsProjectCodeKnownErrors { get; init; } = new();

    public string ProjectCode { get; init; } = null!;

    public List<string> ProjectCodeErrors { get; init; } = new();

    public string ProjectCodeFormElementName => ProjectCodeDto.ProjectCodeFormName;

    public string IsProjectCodeKnownFormElementName => ProjectCodeDto.IsProjectCodeKnownFormName;

    public string IsProjectCodeKnownValueYes => ProjectCodeDto.IsProjectCodeKnownValueYes;

    public string IsProjectCodeKnownValueNo => ProjectCodeDto.IsProjectCodeKnownValueNo;


    public IEnumerable<string> Errors { get; set; } = new List<string>();
}

public sealed class ProjectCodeDto
{
    public const int ProjectCodeMaxLength = 10;
    public const string IsProjectCodeKnownValueYes = "yes";
    public const string IsProjectCodeKnownValueNo = "no";

    public const string IsProjectCodeKnownFormName = "is_project_code_known";
    public const string ProjectCodeFormName = "project_code";

    [FromForm(Name = IsProjectCodeKnownFormName)]
    public string? IsProjectCodeKnown { get; init; } = null;

    [FromForm(Name = ProjectCodeFormName)]
    public string ProjectCodeValue { get; init; } = null!;
}
