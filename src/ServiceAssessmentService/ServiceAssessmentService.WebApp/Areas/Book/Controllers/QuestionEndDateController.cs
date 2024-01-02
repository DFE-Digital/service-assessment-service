using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{
    [HttpGet]
    public async Task<IActionResult> EndDate(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        string? isEndDateKnown = bookingRequest.IsEndDateKnown switch
        {
            true => EndDateDto.IsEndDateKnownValueYes,
            false => EndDateDto.IsEndDateKnownValueNo,
            _ => null,
        };

        var model = new EndDateViewModel
        {
            RequestId = bookingRequestId,

            IsEndDateKnownValue = isEndDateKnown,

            EndDateDayValue = bookingRequest.EndDate?.Day,
            EndDateMonthValue = bookingRequest.EndDate?.Month,
            EndDateYearValue = bookingRequest.EndDate?.Year,

            EndDateErrors = new List<string>(),
            EndDateDayErrors = new List<string>(),
            EndDateMonthErrors = new List<string>(),
            EndDateYearErrors = new List<string>(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EndDate(Guid id, [FromForm] EndDateDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);

        bool? isEndDateKnown = dto.IsEndDateKnown switch
        {
            EndDateDto.IsEndDateKnownValueYes => true,
            EndDateDto.IsEndDateKnownValueNo => false,
            _ => null,
        };

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdateEndDate(bookingRequestId, isEndDateKnown, dto.Year, dto.Month, dto.Day);

        if (bookingRequestChangeResult.HasErrors)
        {
            var model = new EndDateViewModel
            {
                RequestId = bookingRequestId,

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                IsEndDateKnownValue = dto.IsEndDateKnown,
                EndDateDayValue = int.TryParse(dto.Day, out var day) ? day : null,
                EndDateMonthValue = int.TryParse(dto.Month, out var month) ? month : (dto.Month is not null && MonthNameToNumberMapping.ContainsKey(dto.Month?.ToLowerInvariant())) ? MonthNameToNumberMapping[dto.Month?.ToLowerInvariant()] : null,
                EndDateYearValue = int.TryParse(dto.Year, out var year) ? year : null,

                // TODO/FIXME: Doesn't allow for errors on specific parts of the date, instead current implementation has all errors be grouped into a generic "Errors" collection.
                EndDateErrors = bookingRequestChangeResult.Errors.Select(e => e.Message).ToList(),
                // EndDateErrors = endDateErrors,
                // EndDateDayErrors = endDateDayErrors,
                // EndDateMonthErrors = endDateMonthErrors,
                // EndDateYearErrors = endDateYearErrors,
            };

            return View(model);
        }

        return RedirectToAction(nameof(ReviewDates), new { id = bookingRequestId });
    }
}

public sealed class EndDateViewModel
{
    public BookingRequestId RequestId { get; init; }

    public string? IsEndDateKnownValue { get; init; } = null;
    public string IsEndDateKnownFormElementName => EndDateDto.IsEndDateKnownFormName;

    public string IsEndDateKnownValueYes => EndDateDto.IsEndDateKnownValueYes;

    public string IsEndDateKnownValueNo => EndDateDto.IsEndDateKnownValueNo;

    public string EndDateFormElementNamePrefix => EndDateDto.DateFormNamePrefix;

    public string EndDateDayFormElementName => EndDateDto.DayFormName;

    public string EndDateMonthFormElementName => EndDateDto.MonthFormName;

    public string EndDateYearFormElementName => EndDateDto.YearFormName;

    public int? EndDateDayValue { get; init; } = null;

    public int? EndDateMonthValue { get; init; } = null;

    public int? EndDateYearValue { get; init; } = null;


    public DateInputErrorComponents ErrorItems
    {
        get
        {
            // Default to no error components
            var errorItems = DateInputErrorComponents.None;

            if (EndDateDayErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Day;
            }

            if (EndDateMonthErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Month;
            }

            if (EndDateYearErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Year;
            }

            if (EndDateErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.All;
            }

            return errorItems;
        }
    }

    public List<string> IsEndDateKnownErrors { get; init; } = new();
    public List<string> EndDateErrors { get; init; } = new();

    public List<string> EndDateDayErrors { get; init; } = new();

    public List<string> EndDateMonthErrors { get; init; } = new();

    public List<string> EndDateYearErrors { get; init; } = new();

    public List<string> AllErrors => EndDateErrors
        .Concat(EndDateDayErrors)
        .Concat(EndDateMonthErrors)
        .Concat(EndDateYearErrors)
        .Concat(IsEndDateKnownErrors)
        .ToList();

}

public sealed class EndDateDto
{
    public const string IsEndDateKnownFormName = "is_end_date_known";
    public const string DateFormNamePrefix = "end-date";

    public const string DayFormName = DateFormNamePrefix + "." + "Day";
    public const string MonthFormName = DateFormNamePrefix + "." + "Month";
    public const string YearFormName = DateFormNamePrefix + "." + "Year";

    public const string IsEndDateKnownValueYes = "yes";
    public const string IsEndDateKnownValueNo = "no";

    [FromForm(Name = IsEndDateKnownFormName)]
    public string? IsEndDateKnown { get; init; } = null;

    [FromForm(Name = DayFormName)] public string? Day { get; init; } = null;

    [FromForm(Name = MonthFormName)] public string? Month { get; init; } = null;

    [FromForm(Name = YearFormName)] public string? Year { get; init; } = null;

}
