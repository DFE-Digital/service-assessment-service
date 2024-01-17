using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{
    [HttpGet]
    public async Task<IActionResult> StartDate(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var model = new StartDateViewModel
        {
            RequestId = bookingRequestId,

            StartDateDayValue = bookingRequest.StartDate?.Day,
            StartDateMonthValue = bookingRequest.StartDate?.Month,
            StartDateYearValue = bookingRequest.StartDate?.Year,

            StartDateErrors = new List<string>(),
            StartDateDayErrors = new List<string>(),
            StartDateMonthErrors = new List<string>(),
            StartDateYearErrors = new List<string>(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartDate(Guid id, [FromForm] StartDateDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateStartDate(bookingRequestId, dto.Year, dto.Month, dto.Day);

        if (bookingRequestChangeResult.HasErrors)
        {
            var model = new StartDateViewModel
            {
                RequestId = bookingRequestId,

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                StartDateDayValue = int.TryParse(dto.Day, out var day) ? day : null,
                StartDateMonthValue = int.TryParse(dto.Month, out var month) ? month : (dto.Month is not null && MonthNameToNumberMapping.ContainsKey(dto.Month?.ToLowerInvariant())) ? MonthNameToNumberMapping[dto.Month?.ToLowerInvariant()] : null,
                StartDateYearValue = int.TryParse(dto.Year, out var year) ? year : null,

                // TODO/FIXME: Doesn't allow for errors on specific parts of the date, instead current implementation has all errors be grouped into a generic "Errors" collection.
                StartDateErrors = bookingRequestChangeResult.Errors.Select(e => e.Message).ToList(),
                // StartDateErrors = startDateErrors,
                // StartDateDayErrors = startDateDayErrors,
                // StartDateMonthErrors = startDateMonthErrors,
                // StartDateYearErrors = startDateYearErrors,
            };

            return View(model);
        }

        return RedirectToAction(nameof(EndDate), new { id = bookingRequestId });
    }
}

public sealed class StartDateViewModel
{
    public BookingRequestId RequestId { get; init; }

    public string StartDateFormElementNamePrefix => StartDateDto.DateFormNamePrefix;

    public string StartDateDayFormElementName => StartDateDto.DayFormName;

    public string StartDateMonthFormElementName => StartDateDto.MonthFormName;

    public string StartDateYearFormElementName => StartDateDto.YearFormName;

    public int? StartDateDayValue { get; init; } = null;

    public int? StartDateMonthValue { get; init; } = null;

    public int? StartDateYearValue { get; init; } = null;


    public DateInputErrorComponents ErrorItems
    {
        get
        {
            // Default to no error components
            var errorItems = DateInputErrorComponents.None;

            if (StartDateDayErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Day;
            }

            if (StartDateMonthErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Month;
            }

            if (StartDateYearErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Year;
            }

            if (StartDateErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.All;
            }

            return errorItems;
        }
    }

    public List<string> StartDateErrors { get; init; } = new();

    public List<string> StartDateDayErrors { get; init; } = new();

    public List<string> StartDateMonthErrors { get; init; } = new();

    public List<string> StartDateYearErrors { get; init; } = new();

    public List<string> AllErrors => StartDateErrors
        .Concat(StartDateDayErrors)
        .Concat(StartDateMonthErrors)
        .Concat(StartDateYearErrors)
        .ToList();
}

public sealed class StartDateDto
{
    public const string DateFormNamePrefix = "start-date";

    public const string DayFormName = DateFormNamePrefix + "." + "Day";
    public const string MonthFormName = DateFormNamePrefix + "." + "Month";
    public const string YearFormName = DateFormNamePrefix + "." + "Year";

    [FromForm(Name = DayFormName)] public string? Day { get; init; } = null;

    [FromForm(Name = MonthFormName)] public string? Month { get; init; } = null;

    [FromForm(Name = YearFormName)] public string? Year { get; init; } = null;
}
