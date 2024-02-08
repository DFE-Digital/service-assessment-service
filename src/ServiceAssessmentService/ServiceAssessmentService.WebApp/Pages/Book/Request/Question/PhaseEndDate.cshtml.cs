using GovUk.Frontend.AspNetCore.ModelBinding;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class PhaseEndDateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<PhaseEndDateModel> _logger;

    public PhaseEndDateModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<PhaseEndDateModel> logger
    )
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _assessmentTypeRepository = assessmentTypeRepository;
        _phaseRepository = phaseRepository;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }


    public Guid? Id { get; set; } = Guid.Empty;

    public Phase? Phase { get; set; } = null;

    [BindProperty]
    public string? IsEndDateKnownValue { get; set; }

    [BindProperty]
    public int? EndDateDayValue { get; set; }
    [BindProperty]
    public int? EndDateMonthValue { get; set; }
    [BindProperty]
    public int? EndDateYearValue { get; set; }


    public List<string> RadioErrors { get; set; } = new();

    public List<string> DayErrors { get; set; } = new();
    public List<string> MonthErrors { get; set; } = new();
    public List<string> YearErrors { get; set; } = new();
    public List<string> DateErrors { get; set; } = new();
    public List<string> AllDateErrors => DayErrors.Concat(MonthErrors).Concat(YearErrors).Concat(DateErrors).ToList();


    public DateInputErrorComponents ErrorItems
    {
        get
        {
            // Default to no error components
            var errorItems = DateInputErrorComponents.None;

            if (DayErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Day;
            }

            if (MonthErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Month;
            }

            if (YearErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.Year;
            }

            if (DateErrors.Count > 0)
            {
                errorItems |= DateInputErrorComponents.All;
            }

            return errorItems;
        }
    }


    public List<string> DayWarnings { get; set; } = new();
    public List<string> MonthWarnings { get; set; } = new();
    public List<string> YearWarnings { get; set; } = new();
    public List<string> DateWarnings { get; set; } = new();
    public List<string> AllWarnings => DayWarnings.Concat(MonthWarnings).Concat(YearWarnings).Concat(DateWarnings).ToList();




    private const string _dateFormNamePrefix = "service-phase-end-date";
    public string DateFormNamePrefix => _dateFormNamePrefix;

    public string RadioQuestionText => $"Do you have an end date for your {Phase?.DisplayNameMidSentenceCase ?? "phase"}?";
    public string RadioQuestionHint => $"Select one option.";

    public string DateQuestionText => $"When will your {Phase?.DisplayNameMidSentenceCase ?? "phase"} end?";
    public string DateQuestionHint => $"For example, 18 2 2023.";



    private const string _isDateKnownFormElementName = "service-is-phase-end-date-known";
    public string IsDateKnownFormElementName => _isDateKnownFormElementName;

    private const string _isDateKnownRadioPrefix = "is-phase-end-date-known";
    public string IsDateKnownRadioPrefix => _isDateKnownRadioPrefix;

    public const string _isEndDateKnownValueYes = "yes";
    public String IsEndDateKnownValueYes => _isEndDateKnownValueYes;

    public const string _isEndDateKnownValueNo = "no";
    public String IsEndDateKnownValueNo => _isEndDateKnownValueNo;


    public async Task<IActionResult> OnGet(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        Id = id;
        Phase = req.PhaseConcluding;
        IsEndDateKnownValue = req.IsPhaseEndDateKnown switch
        {
            true => _isEndDateKnownValueYes,
            false => _isEndDateKnownValueNo,
            _ => null,
        };
        EndDateYearValue = req.PhaseEndDate?.Year;
        EndDateMonthValue = req.PhaseEndDate?.Month;
        EndDateDayValue = req.PhaseEndDate?.Day;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _isDateKnownFormElementName), AllowEmpty] string? newIsDateKnown,
        [FromForm(Name = _dateFormNamePrefix + ".Day"), AllowEmpty] string? newDay,
        [FromForm(Name = _dateFormNamePrefix + ".Month"), AllowEmpty] string? newMonth,
        [FromForm(Name = _dateFormNamePrefix + ".Year"), AllowEmpty] string? newYear
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }


        bool? isEndDateKnown = newIsDateKnown switch
        {
            _isEndDateKnownValueYes => true,
            _isEndDateKnownValueNo => false,
            _ => null,
        };

        var changeResult = await _assessmentRequestRepository.UpdateEndDateByPartsAsync(id, isEndDateKnown, newYear, newMonth, newDay);
        if (!changeResult.IsValid || !changeResult.NestedValidationResult.IsValid)
        {
            var yearIsInt = int.TryParse(newYear, out int year);
            var monthIsInt = int.TryParse(newMonth, out int month);
            var dayIsInt = int.TryParse(newDay, out int day);

            Id = id;
            Phase = req.PhaseConcluding;
            IsEndDateKnownValue = newIsDateKnown;
            EndDateYearValue = yearIsInt ? year : null;   // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            EndDateMonthValue = monthIsInt ? month : null;   // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            EndDateDayValue = dayIsInt ? day : null;   // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            RadioErrors = changeResult.RadioQuestionValidationErrors.Select(e => e.ErrorMessage).ToList();

            DayErrors = changeResult.NestedValidationResult.DayValidationErrors.Select(e => e.ErrorMessage).ToList();
            MonthErrors = changeResult.NestedValidationResult.MonthValidationErrors.Select(e => e.ErrorMessage).ToList();
            YearErrors = changeResult.NestedValidationResult.YearValidationErrors.Select(e => e.ErrorMessage).ToList();
            DateErrors = changeResult.NestedValidationResult.DateValidationErrors.Select(e => e.ErrorMessage).ToList();

            DayWarnings = changeResult.NestedValidationResult.DayValidationWarnings.Select(e => e.WarningMessage).ToList();
            MonthWarnings = changeResult.NestedValidationResult.MonthValidationWarnings.Select(e => e.WarningMessage).ToList();
            YearWarnings = changeResult.NestedValidationResult.YearValidationWarnings.Select(e => e.WarningMessage).ToList();
            DateWarnings = changeResult.NestedValidationResult.DateValidationWarnings.Select(e => e.WarningMessage).ToList();

            return Page();
        }

        return RedirectToPage("/Book/Request/Question/Portfolio", new { id });
    }
}
