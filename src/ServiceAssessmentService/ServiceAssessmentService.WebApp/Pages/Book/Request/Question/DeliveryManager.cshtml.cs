using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class DeliveryManagerPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<DeliveryManagerPageModel> _logger;

    public DeliveryManagerPageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<DeliveryManagerPageModel> logger
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
    public string? HasDeliveryManagerValue { get; set; }

    [BindProperty]
    public string? DeliveryManagerPersonalName { get; set; }
    [BindProperty]
    public string? DeliveryManagerFamilyName { get; set; }
    [BindProperty]
    public string? DeliveryManagerEmail { get; set; }


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



    public const string _hasDeliveryManagerValueYes = "yes";
    public String HasDeliveryManagerValueYes => _hasDeliveryManagerValueYes;

    public const string _hasDeliveryManagerValueNo = "no";
    public String HasDeliveryManagerValueNo => _hasDeliveryManagerValueNo;

    private const string _hasDeliveryManagerFormElementName = "service-has-delivery-manager";
    public string HasDeliveryManagerFormElementName => _hasDeliveryManagerFormElementName;

    private const string _hasDeliveryManagerRadioPrefix = "has-delivery-manager";
    public string HasDeliveryManagerRadioPrefix => _hasDeliveryManagerRadioPrefix;

    private const string _formElementNameDeliveryManagerPersonalName = "service-delivery-manager-personal-name";
    public string FormElementNameDeliveryManagerPersonalName => _formElementNameDeliveryManagerPersonalName;

    private const string _formElementNameDeliveryManagerFamilyName = "service-delivery-manager-family-name";
    public string FormElementNameDeliveryManagerFamilyName => _formElementNameDeliveryManagerFamilyName;

    private const string _formElementNameDeliveryManagerEmail = "service-delivery-manager-email";
    public string FormElementNameDeliveryManagerEmail => _formElementNameDeliveryManagerEmail;




    public string RadioQuestionText => $"Does the team have a delivery manager?";
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

        HasDeliveryManagerValue = req.HasDeliveryManager switch
        {
            true => _hasDeliveryManagerValueYes,
            false => _hasDeliveryManagerValueNo,
            _ => null,
        };

        DeliveryManagerPersonalName = req.DeliveryManager?.PersonalName ?? string.Empty;
        DeliveryManagerFamilyName = req.DeliveryManager?.FamilyName ?? string.Empty;
        DeliveryManagerEmail = req.DeliveryManager?.Email ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _hasDeliveryManagerFormElementName), AllowEmpty] string? newHasDeliveryManager,
        [FromForm(Name = _formElementNameDeliveryManagerPersonalName), AllowEmpty] string newPersonalName,
        [FromForm(Name = _formElementNameDeliveryManagerFamilyName), AllowEmpty] string newFamilyName,
        [FromForm(Name = _formElementNameDeliveryManagerEmail), AllowEmpty] string newEmail
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }


        bool? hasDeliveryManager = newHasDeliveryManager switch
        {
            _hasDeliveryManagerValueYes => true,
            _hasDeliveryManagerValueNo => false,
            _ => null,
        };

        var changeResult = await _assessmentRequestRepository.UpdateDeliveryManagerAsync(id, hasDeliveryManager, newPersonalName, newFamilyName, newEmail);
        if (!changeResult.IsValid)
        {
            Id = id;

            HasDeliveryManagerValue = newHasDeliveryManager; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            DeliveryManagerPersonalName = newPersonalName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            DeliveryManagerFamilyName = newFamilyName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            DeliveryManagerEmail = newEmail; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

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
        // return RedirectToPage("/Book/Request/Question/DeliveryManager", new { id });
    }
}
