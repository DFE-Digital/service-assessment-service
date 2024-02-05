using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request.Question;

public class ProjectCode : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<ProjectCode> _logger;

    public ProjectCode(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<ProjectCode> logger
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
    public string? IsProjectCodeKnownValue { get; set; }

    [BindProperty]
    public string? ProjectCodeValue { get; set; }

    public List<string> RadioErrors { get; set; } = new();

    public List<string> ProjectCodeErrors { get; set; } = new();


    private const string _isProjectCodeKnownFormElementName = "service-is-project-code-known";
    public string IsProjectCodeKnownFormElementName => _isProjectCodeKnownFormElementName;

    private const string _isProjectCodeKnownRadioPrefix = "is-project-code-known";
    public string IsProjectCodeKnownRadioPrefix => _isProjectCodeKnownRadioPrefix;

    private const string _projectCodeFormElementName = "service-project-code";
    public string ProjectCodeFormElementName => _projectCodeFormElementName;


    public string RadioQuestionText => "Do you know your project code?";
    public string? RadioQuestionHint => @"This code is sometimes called a project ID. It starts with DDaT.
                                For example, <code>DDaT_22/23_001</code>.</p>
                                Find the code on the <a href=""https://educationgovuk.sharepoint.com/:x:/r/sites/efarafdg/c/_layouts/15/Doc.aspx?sourcedoc=%7B7086CF76-D57F-41B5-92E3-853C107AB68F%7D&amp;file=DDaT%20Portfolio%20Tracker.xlsx&amp;action=default&amp;mobileredirect=true&amp;DefaultItemOpen=1&amp;cid=f4f6cbd1-a864-4102-aed3-962ca11d120f"" class=""govuk-link"" rel=""noreferrer noopener"" target=""_blank"">DDaT portfolio tracker (opens in new tab)</a>, or speak to your <a href=""https://educationgovuk.sharepoint.com/sites/lvewp00038/SitePages/DDaT-Business-Partners.aspx"" class=""govuk-link"" rel=""noreferrer noopener"" target=""_blank"">business partner (opens in new tab)</a>.
                                Select one option.";

    public const string _isProjectCodeKnownValueYes = "yes";
    public String IsProjectCodeKnownValueYes => _isProjectCodeKnownValueYes;

    public const string _isProjectCodeKnownValueNo = "no";
    public String IsProjectCodeKnownValueNo => _isProjectCodeKnownValueNo;


    public string ProjectCodeQuestionText => "What is your project code?";
    public string? ProjectCodeQuestionHint => null; // "Enter the project code.";

    public async Task<IActionResult> OnGet(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        Id = id;
        IsProjectCodeKnownValue = req.IsProjectCodeKnown switch
        {
            true => _isProjectCodeKnownValueYes,
            false => _isProjectCodeKnownValueNo,
            _ => null,
        };
        ProjectCodeValue = req.ProjectCode;

        return Page();
    }

    public async Task<IActionResult> OnPost(
        Guid id,
        [FromForm(Name = _isProjectCodeKnownFormElementName)] string? isProjectCodeKnownValue,
        [FromForm(Name = _projectCodeFormElementName)] string? projectCodeValue
    )
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        bool? isProjectCodeKnown = isProjectCodeKnownValue switch
        {
            _isProjectCodeKnownValueYes => true,
            _isProjectCodeKnownValueNo => false,
            _ => null,
        };

        var changeResult =
            await _assessmentRequestRepository.UpdateProjectCode(id, isProjectCodeKnown, projectCodeValue);
        if (!changeResult.IsValid || !changeResult.NestedValidationResult.IsValid)
        {
            Id = id;
            IsProjectCodeKnownValue = isProjectCodeKnownValue; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.
            ProjectCodeValue = projectCodeValue; // Update the page model to use the user-supplied value, allowing them to edit it on the next page rather than losing their input.

            RadioErrors = changeResult.RadioQuestionValidationErrors.Select(e => e.ErrorMessage).ToList();
            ProjectCodeErrors = changeResult.NestedValidationResult.ValidationErrors.Select(e => e.ErrorMessage).ToList();

            return Page();
        }

        //return RedirectToPage("/Book/Request/TaskList", new { id });
        return RedirectToPage("/Book/Request/Question/PhaseStartDate", new { id });
    }
}
