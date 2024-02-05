using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.WebApp.Pages.Shared;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class AssessmentTypePageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<AssessmentTypePageModel> _logger;

    public AssessmentTypePageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<AssessmentTypePageModel> logger
    )
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _assessmentTypeRepository = assessmentTypeRepository;
        _phaseRepository = phaseRepository;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    public Guid? Id { get; set; } = Guid.Empty;


    public IEnumerable<AssessmentType> AssessmentTypes { get; set; } = new List<AssessmentType>();

    public IEnumerable<RadioItem> AvailableAssessmentTypes { get; set; } = new List<RadioItem>();

    [BindProperty]
    public string? SelectedAssessmentTypeId { get; set; } = null;


    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();


    private const string _formElementName = "service-assessment-type";
    public string FormElementName => _formElementName;

    public string QuestionText => $"What type of assessment are you requesting?";

    public string? QuestionHint => null;

    public async Task<IActionResult> OnGet(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        AssessmentTypes = await _assessmentTypeRepository.GetAssessmentTypesAsync();
        AvailableAssessmentTypes = AssessmentTypes.Select(x => new RadioItem(x.Id.ToString(), x.Name, true, x.SortOrder));

        Id = req.Id;
        SelectedAssessmentTypeId = req.AssessmentType?.Id.ToString();

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _formElementName), AllowEmpty]
        string? newAssessmentTypeId
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        // TODO: Consider only setting the assessment type once (rejecting any attempts to set/update it if it's already set).

        var changeResult = await _assessmentRequestRepository.UpdateAssessmentTypeAsync(req.Id, newAssessmentTypeId);
        if (!changeResult.IsValid || changeResult.ValidationErrors.Any())
        {

            AssessmentTypes = await _assessmentTypeRepository.GetAssessmentTypesAsync();
            AvailableAssessmentTypes = AssessmentTypes.Select(x => new RadioItem(x.Id.ToString(), x.Name, true, x.SortOrder));

            Id = req.Id;
            SelectedAssessmentTypeId = req.AssessmentType?.Id.ToString(); // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            Warnings = changeResult.ValidationWarnings.Select(e => e.WarningMessage).ToList();
            Errors = changeResult.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            return Page();
        }

        //return RedirectToPage("/Book/Request/TaskList", new { id });
        return RedirectToPage("/Book/Request/Question/ProjectCode", new { id });
    }

}
