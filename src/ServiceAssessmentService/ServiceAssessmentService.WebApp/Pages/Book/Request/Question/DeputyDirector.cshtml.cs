using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class DeputyDirectorPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<DeputyDirectorPageModel> _logger;

    public DeputyDirectorPageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<DeputyDirectorPageModel> logger
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
    public string? DeputyDirectorPersonalName { get; set; }
    [BindProperty]
    public string? DeputyDirectorFamilyName { get; set; }
    [BindProperty]
    public string? DeputyDirectorEmail { get; set; }


    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ValidationWarnings { get; set; } = new();

    public List<string> PersonalNameErrors { get; set; } = new();
    public List<string> PersonalNameWarnings { get; set; } = new();

    public List<string> FamilyNameErrors { get; set; } = new();
    public List<string> FamilyNameWarnings { get; set; } = new();

    public List<string> EmailErrors { get; set; } = new();
    public List<string> EmailWarnings { get; set; } = new();


    private const string _formElementNameDeputyDirectorPersonalName = "service-deputy-director-personal-name";
    public string FormElementNameDeputyDirectorPersonalName => _formElementNameDeputyDirectorPersonalName;

    private const string _formElementNameDeputyDirectorFamilyName = "service-deputy-director-family-name";
    public string FormElementNameDeputyDirectorFamilyName => _formElementNameDeputyDirectorFamilyName;

    private const string _formElementNameDeputyDirectorEmail = "service-deputy-director-email";
    public string FormElementNameDeputyDirectorEmail => _formElementNameDeputyDirectorEmail;

    public string QuestionText => $"Who is the digital deputy director of your portfolio?";
    public string? QuestionHint => null;

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

        DeputyDirectorPersonalName = req.DeputyDirector?.PersonalName ?? string.Empty;
        DeputyDirectorFamilyName = req.DeputyDirector?.FamilyName ?? string.Empty;
        DeputyDirectorEmail = req.DeputyDirector?.Email ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _formElementNameDeputyDirectorPersonalName), AllowEmpty] string newPersonalName,
        [FromForm(Name = _formElementNameDeputyDirectorFamilyName), AllowEmpty] string newFamilyName,
        [FromForm(Name = _formElementNameDeputyDirectorEmail), AllowEmpty] string newEmail
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        var changeResult = await _assessmentRequestRepository.UpdateDeputyDirectorAsync(id, newPersonalName, newFamilyName, newEmail);
        if (!changeResult.IsValid)
        {
            Id = id;

            DeputyDirectorPersonalName = newPersonalName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            DeputyDirectorFamilyName = newFamilyName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            DeputyDirectorEmail = newEmail; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            ValidationWarnings = changeResult.ValidationWarnings.Select(e => e.WarningMessage).ToList();
            ValidationErrors = changeResult.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            PersonalNameWarnings = changeResult.PersonalNameWarnings.Select(e => e.WarningMessage).ToList();
            PersonalNameErrors = changeResult.PersonalNameErrors.Select(e => e.ErrorMessage).ToList();
            FamilyNameWarnings = changeResult.FamilyNameWarnings.Select(e => e.WarningMessage).ToList();
            FamilyNameErrors = changeResult.FamilyNameErrors.Select(e => e.ErrorMessage).ToList();
            EmailWarnings = changeResult.EmailWarnings.Select(e => e.WarningMessage).ToList();
            EmailErrors = changeResult.EmailErrors.Select(e => e.ErrorMessage).ToList();

            return Page();
        }

        //return RedirectToPage("/Book/Request/TaskList", new { id });
        return RedirectToPage("/Book/Request/Question/SeniorResponsibleOfficer", new { id });
    }
}
