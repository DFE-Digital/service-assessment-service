using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class ProductOwnerManagerPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<ProductOwnerManagerPageModel> _logger;

    public ProductOwnerManagerPageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<ProductOwnerManagerPageModel> logger
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
    public string? HasProductOwnerManagerValue { get; set; }

    [BindProperty]
    public string? ProductOwnerManagerPersonalName { get; set; }
    [BindProperty]
    public string? ProductOwnerManagerFamilyName { get; set; }
    [BindProperty]
    public string? ProductOwnerManagerEmail { get; set; }


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



    public const string _hasProductOwnerManagerValueYes = "yes";
    public String HasProductOwnerManagerValueYes => _hasProductOwnerManagerValueYes;

    public const string _hasProductOwnerManagerValueNo = "no";
    public String HasProductOwnerManagerValueNo => _hasProductOwnerManagerValueNo;

    private const string _hasProductOwnerManagerFormElementName = "service-has-product-owner-manager";
    public string HasProductOwnerManagerFormElementName => _hasProductOwnerManagerFormElementName;

    private const string _hasProductOwnerManagerRadioPrefix = "has-product-owner-manager";
    public string HasProductOwnerManagerRadioPrefix => _hasProductOwnerManagerRadioPrefix;

    private const string _formElementNameProductOwnerManagerPersonalName = "service-product-owner-manager-personal-name";
    public string FormElementNameProductOwnerManagerPersonalName => _formElementNameProductOwnerManagerPersonalName;

    private const string _formElementNameProductOwnerManagerFamilyName = "service-product-owner-manager-family-name";
    public string FormElementNameProductOwnerManagerFamilyName => _formElementNameProductOwnerManagerFamilyName;

    private const string _formElementNameProductOwnerManagerEmail = "service-product-owner-manager-email";
    public string FormElementNameProductOwnerManagerEmail => _formElementNameProductOwnerManagerEmail;




    public string RadioQuestionText => $"Does the team have a product owner or product manager?";
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

        HasProductOwnerManagerValue = req.HasProductOwnerManager switch
        {
            true => _hasProductOwnerManagerValueYes,
            false => _hasProductOwnerManagerValueNo,
            _ => null,
        };

        ProductOwnerManagerPersonalName = req.ProductOwnerManager?.PersonalName ?? string.Empty;
        ProductOwnerManagerFamilyName = req.ProductOwnerManager?.FamilyName ?? string.Empty;
        ProductOwnerManagerEmail = req.ProductOwnerManager?.Email ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _hasProductOwnerManagerFormElementName), AllowEmpty] string? newHasProductOwnerManager,
        [FromForm(Name = _formElementNameProductOwnerManagerPersonalName), AllowEmpty] string newPersonalName,
        [FromForm(Name = _formElementNameProductOwnerManagerFamilyName), AllowEmpty] string newFamilyName,
        [FromForm(Name = _formElementNameProductOwnerManagerEmail), AllowEmpty] string newEmail
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }


        bool? hasProductOwnerManager = newHasProductOwnerManager switch
        {
            _hasProductOwnerManagerValueYes => true,
            _hasProductOwnerManagerValueNo => false,
            _ => null,
        };

        var changeResult = await _assessmentRequestRepository.UpdateProductOwnerManagerAsync(id, hasProductOwnerManager, newPersonalName, newFamilyName, newEmail);
        if (!changeResult.IsValid)
        {
            Id = id;

            HasProductOwnerManagerValue = newHasProductOwnerManager; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            ProductOwnerManagerPersonalName = newPersonalName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            ProductOwnerManagerFamilyName = newFamilyName; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            ProductOwnerManagerEmail = newEmail; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

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
