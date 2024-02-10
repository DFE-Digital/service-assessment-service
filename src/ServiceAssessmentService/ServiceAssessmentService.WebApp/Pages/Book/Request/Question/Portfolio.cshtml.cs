using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.WebApp.Pages.Shared;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class PortfolioPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<PortfolioPageModel> _logger;

    public PortfolioPageModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<PortfolioPageModel> logger
    )
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _assessmentTypeRepository = assessmentTypeRepository;
        _phaseRepository = phaseRepository;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    public Guid? Id { get; set; } = Guid.Empty;


    public IEnumerable<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

    public IEnumerable<RadioItem> AvailableInternalPortfolios { get; set; } = new List<RadioItem>();
    public IEnumerable<RadioItem> AvailableExternalPortfolios { get; set; } = new List<RadioItem>();

    [BindProperty]
    public string? SelectedPortfolioId { get; set; } = null;


    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();


    private const string _formElementName = "service-portfolio";
    public string FormElementName => _formElementName;

    public string QuestionText => $"Which DfE group or arms-length body is your assessment for?";

    public string? QuestionHint => "You may know these as portfolios.";

    public async Task<IActionResult> OnGet(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        Portfolios = (await _portfolioRepository.GetPortfoliosAsync()).ToList();
        AvailableInternalPortfolios = Portfolios
            .Where(x => x.IsInternalGroup)
            .Select(x => new RadioItem(x.Id.ToString(), x.Name, true, x.SortOrder));
        AvailableExternalPortfolios = Portfolios
            .Where(x => !x.IsInternalGroup)
            .Select(x => new RadioItem(x.Id.ToString(), x.Name, true, x.SortOrder));

        Id = req.Id;
        SelectedPortfolioId = req.Portfolio?.Id.ToString();

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _formElementName), AllowEmpty]
        string? newPortfolioId
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        var changeResult = await _assessmentRequestRepository.UpdatePortfolioAsync(req.Id, newPortfolioId);
        if (!changeResult.IsValid || changeResult.ValidationErrors.Any())
        {

            Portfolios = (await _portfolioRepository.GetPortfoliosAsync()).ToList();
            AvailableInternalPortfolios = Portfolios
                .Where(x => x.IsInternalGroup)
                .Select(x => new RadioItem(x.Id.ToString(), x.Name, true, x.SortOrder));
            AvailableExternalPortfolios = Portfolios
                .Where(x => !x.IsInternalGroup)
                .Select(x => new RadioItem(x.Id.ToString(), x.Name, true, x.SortOrder));

            Id = req.Id;
            SelectedPortfolioId = req.Portfolio?.Id.ToString(); // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            Warnings = changeResult.ValidationWarnings.Select(e => e.WarningMessage).ToList();
            Errors = changeResult.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            return Page();
        }

        // return RedirectToPage("/Book/Request/TaskList", new { id });
        return RedirectToPage("/Book/Request/Question/DeputyDirector", new { id });
    }

}
