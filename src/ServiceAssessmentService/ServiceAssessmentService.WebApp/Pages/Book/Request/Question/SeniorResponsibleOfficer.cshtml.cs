using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class SeniorResponsibleOfficerPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<SeniorResponsibleOfficerPageModel> _logger;

    public SeniorResponsibleOfficerPageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<SeniorResponsibleOfficerPageModel> logger
    )
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _assessmentTypeRepository = assessmentTypeRepository;
        _phaseRepository = phaseRepository;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }


    public Guid? Id { get; set; } = Guid.Empty;

    [BindProperty]
    public string? IsDdTheSroValue { get; set; }

    [BindProperty]
    public string? SeniorResponsibleOfficerPersonalName { get; set; }
    [BindProperty]
    public string? SeniorResponsibleOfficerFamilyName { get; set; }
    [BindProperty]
    public string? SeniorResponsibleOfficerEmail { get; set; }


    public List<string> RadioErrors { get; set; } = new();
    public List<string> RadioWarnings { get; set; } = new();

    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ValidationWarnings { get; set; } = new();

    public List<string> PersonalNameErrors { get; set; } = new();
    public List<string> PersonalNameWarnings { get; set; } = new();

    public List<string> FamilyNameErrors { get; set; } = new();
    public List<string> FamilyNameWarnings { get; set; } = new();

    public List<string> EmailErrors { get; set; } = new();
    public List<string> EmailWarnings { get; set; } = new();



    public const string _isDdTheSroValueYes = "yes";
    public String IsDdTheSroValueYes => _isDdTheSroValueYes;

    public const string _isDdTheSroValueNo = "no";
    public String IsDdTheSroValueNo => _isDdTheSroValueNo;

    private const string _isDdTheSroFormElementName = "service-is-dd-the-sro";
    public string IsDdTheSroFormElementName => _isDdTheSroFormElementName;

    private const string _isDdTheSroRadioPrefix = "is-dd-the-sro";
    public string IsDdTheSroRadioPrefix => _isDdTheSroRadioPrefix;

    private const string _formElementNameSeniorResponsibleOfficerPersonalName = "service-deputy-director-personal-name";
    public string FormElementNameSeniorResponsibleOfficerPersonalName => _formElementNameSeniorResponsibleOfficerPersonalName;

    private const string _formElementNameSeniorResponsibleOfficerFamilyName = "service-deputy-director-family-name";
    public string FormElementNameSeniorResponsibleOfficerFamilyName => _formElementNameSeniorResponsibleOfficerFamilyName;

    private const string _formElementNameSeniorResponsibleOfficerEmail = "service-deputy-director-email";
    public string FormElementNameSeniorResponsibleOfficerEmail => _formElementNameSeniorResponsibleOfficerEmail;




    public string RadioQuestionText => $"Is your deputy director also the senior responsible officer (SRO)?";
    public string? RadioQuestionHint => "Select one option.";

    public string PersonalNameQuestionText => $"Personal name";
    public string? PersonalNameQuestionHint => null;

    public string FamilyNameQuestionText => $"Family name";
    public string? FamilyNameQuestionHint => null;

    public string EmailQuestionText => $"Email address";
    public string? EmailQuestionHint => null;


    public async Task<IActionResult> OnGet(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        Id = id;

        IsDdTheSroValue = req.IsDeputyDirectorTheSeniorResponsibleOfficer switch
        {
            true => _isDdTheSroValueYes,
            false => _isDdTheSroValueNo,
            _ => null,
        };

        SeniorResponsibleOfficerPersonalName = req.SeniorResponsibleOfficer?.PersonalName ?? string.Empty;
        SeniorResponsibleOfficerFamilyName = req.SeniorResponsibleOfficer?.FamilyName ?? string.Empty;
        SeniorResponsibleOfficerEmail = req.SeniorResponsibleOfficer?.Email ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _isDdTheSroFormElementName), AllowEmpty] string? newIsDdTheSro,
        [FromForm(Name = _formElementNameSeniorResponsibleOfficerPersonalName), AllowEmpty] string newPersonalName,
        [FromForm(Name = _formElementNameSeniorResponsibleOfficerFamilyName), AllowEmpty] string newFamilyName,
        [FromForm(Name = _formElementNameSeniorResponsibleOfficerEmail), AllowEmpty] string newEmail
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }


        bool? isDdTheSro = newIsDdTheSro switch
        {
            _isDdTheSroValueYes => true,
            _isDdTheSroValueNo => false,
            _ => null,
        };

        var changeResult = await _assessmentRequestRepository.UpdateSeniorResponsibleOfficerAsync(id, isDdTheSro, newPersonalName, newFamilyName, newEmail);
        if (!changeResult.IsValid)
        {
            Id = id;

            IsDdTheSroValue = newIsDdTheSro; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            SeniorResponsibleOfficerPersonalName = newPersonalName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            SeniorResponsibleOfficerFamilyName = newFamilyName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            SeniorResponsibleOfficerEmail = newEmail; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            RadioWarnings = changeResult.RadioQuestionValidationWarnings.Select(e => e.WarningMessage).ToList();
            RadioErrors = changeResult.RadioQuestionValidationErrors.Select(e => e.ErrorMessage).ToList();

            ValidationWarnings = changeResult.NestedValidationResult.ValidationWarnings.Select(e => e.WarningMessage).ToList();
            ValidationErrors = changeResult.NestedValidationResult.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            PersonalNameWarnings = changeResult.NestedValidationResult.PersonalNameWarnings.Select(e => e.WarningMessage).ToList();
            PersonalNameErrors = changeResult.NestedValidationResult.PersonalNameErrors.Select(e => e.ErrorMessage).ToList();
            FamilyNameWarnings = changeResult.NestedValidationResult.FamilyNameWarnings.Select(e => e.WarningMessage).ToList();
            FamilyNameErrors = changeResult.NestedValidationResult.FamilyNameErrors.Select(e => e.ErrorMessage).ToList();
            EmailWarnings = changeResult.NestedValidationResult.EmailWarnings.Select(e => e.WarningMessage).ToList();
            EmailErrors = changeResult.NestedValidationResult.EmailErrors.Select(e => e.ErrorMessage).ToList();

            return Page();
        }

        return RedirectToPage("/Book/Request/TaskList", new { id });
        // return RedirectToPage("/Book/Request/Question/Description", new { id });
    }
}
