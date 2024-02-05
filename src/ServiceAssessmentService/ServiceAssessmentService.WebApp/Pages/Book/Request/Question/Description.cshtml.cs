using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class DescriptionPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<NamePageModel> _logger;

    public DescriptionPageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<NamePageModel> logger
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
    public string? Description { get; set; } = string.Empty;


    public int MaxLength => AssessmentRequest.DescriptionMaxLengthChars;

    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();


    private const string _formElementName = "service-description";
    public string FormElementName => _formElementName;

    public string QuestionText => $"What is the purpose of your {Phase?.DisplayNameMidSentenceCase ?? "service"}?";

    public string? QuestionHint => $"Tell us the purpose of your {Phase?.DisplayNameMidSentenceCase ?? "service"}." +
                            "For example, if it's part of a wider service or based on policy intent." +
                            "This description will help us to arrange a review panel with the most relevant experience.";


    public async Task<IActionResult> OnGet(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        Id = req.Id;
        Description = req.Description;
        Phase = req.PhaseConcluding;

        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, [FromForm(Name = _formElementName), AllowEmpty] string newDescription)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        var changeResult = await _assessmentRequestRepository.UpdateDescriptionAsync(id, newDescription);
        if (!changeResult.IsValid || changeResult.ValidationErrors.Any())
        {
            Id = req.Id;
            Description = newDescription; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            Warnings = changeResult.ValidationWarnings.Select(e => e.WarningMessage).ToList();
            Errors = changeResult.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            return Page();
        }

        //return RedirectToPage("/Book/Request/TaskList", new { id });
        return RedirectToPage("/Book/Request/Question/ProjectCode", new { id });
    }
}
