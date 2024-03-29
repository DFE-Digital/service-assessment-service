﻿using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class PhaseStartDateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<PhaseStartDateModel> _logger;

    public PhaseStartDateModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<PhaseStartDateModel> logger
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
    public int? StartDateDayValue { get; set; }
    [BindProperty]
    public int? StartDateMonthValue { get; set; }
    [BindProperty]
    public int? StartDateYearValue { get; set; }


    public List<string> DayErrors { get; set; } = new();
    public List<string> MonthErrors { get; set; } = new();
    public List<string> YearErrors { get; set; } = new();
    public List<string> DateErrors { get; set; } = new();
    public List<string> AllErrors => DayErrors.Concat(MonthErrors).Concat(YearErrors).Concat(DateErrors).ToList();


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


    // private const string _formElementName = "service-phase-start-date";
    // public string FormElementName => _formElementName;

    private const string _dateFormNamePrefix = "service-phase-start-date";
    public string DateFormNamePrefix => _dateFormNamePrefix;

    public string QuestionText => $"When did your {Phase?.DisplayNameMidSentenceCase ?? "phase"} start?";

    public string? QuestionHint => $"For example, 18 2 2023.";


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
        StartDateYearValue = req.PhaseStartDate?.Year;
        StartDateMonthValue = req.PhaseStartDate?.Month;
        StartDateDayValue = req.PhaseStartDate?.Day;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
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

        var changeResult = await _assessmentRequestRepository.UpdateStartDateByPartsAsync(id, newYear, newMonth, newDay);
        if (!changeResult.IsValid)
        {
            var yearIsInt = int.TryParse(newYear, out int year);
            var monthIsInt = int.TryParse(newMonth, out int month);
            var dayIsInt = int.TryParse(newDay, out int day);

            Id = id;
            Phase = req.PhaseConcluding;
            StartDateYearValue = yearIsInt ? year : null;   // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            StartDateMonthValue = monthIsInt ? month : null;   // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            StartDateDayValue = dayIsInt ? day : null;   // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            DayErrors = changeResult.DayValidationErrors.Select(e => e.ErrorMessage).ToList();
            MonthErrors = changeResult.MonthValidationErrors.Select(e => e.ErrorMessage).ToList();
            YearErrors = changeResult.YearValidationErrors.Select(e => e.ErrorMessage).ToList();
            DateErrors = changeResult.DateValidationErrors.Select(e => e.ErrorMessage).ToList();

            DayWarnings = changeResult.DayValidationWarnings.Select(e => e.WarningMessage).ToList();
            MonthWarnings = changeResult.MonthValidationWarnings.Select(e => e.WarningMessage).ToList();
            YearWarnings = changeResult.YearValidationWarnings.Select(e => e.WarningMessage).ToList();
            DateWarnings = changeResult.DateValidationWarnings.Select(e => e.WarningMessage).ToList();

            return Page();
        }

        //return RedirectToPage("/Book/Request/TaskList", new { id });
        return RedirectToPage("/Book/Request/Question/PhaseEndDate", new { id });
    }
}
