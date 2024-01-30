using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class CreateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<CreateModel> logger
    )
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _assessmentTypeRepository = assessmentTypeRepository;
        _phaseRepository = phaseRepository;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    public IEnumerable<AssessmentType> AssessmentTypes { get; set; }

    public IEnumerable<Phase> Phases { get; set; }


    [BindProperty]
    public NewAssessmentRequestFormModel? AssessmentRequestPageModel { get; set; }

    public async Task<IActionResult> OnGet()
    {
        Phases = await _phaseRepository.GetPhasesAsync();
        AssessmentTypes = await _assessmentTypeRepository.GetAssessmentTypesAsync();

        // If null, initialise an empty request model (this models the HTTP/form values, later to be mapped into a domain model)
        AssessmentRequestPageModel ??= new NewAssessmentRequestFormModel();

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogDebug("Submitted ModelState is invalid");
            return Page();
        }

        if (AssessmentRequestPageModel is null)
        {
            _logger.LogWarning("Submitted AssessmentRequestPageModel is null");
            return Page();
        }

        Phases = await _phaseRepository.GetPhasesAsync();
        AssessmentTypes = await _assessmentTypeRepository.GetAssessmentTypesAsync();


        var assessmentRequest = AssessmentRequestPageModel.ToDomainModel(Phases, AssessmentTypes);
        assessmentRequest.Id = Guid.NewGuid();

        await _assessmentRequestRepository.CreateAsync(assessmentRequest);

        return RedirectToPage(nameof(View), new { assessmentRequest.Id });
    }


    public class NewAssessmentRequestFormModel
    {
        public string Name { get; set; } = string.Empty;

        public string? SelectedPhaseConcludingId { get; set; }

        public string? SelectedAssessmentTypeId { get; set; }

        public DateOnly? PhaseStartDate { get; set; }

        public DateOnly? PhaseEndDate { get; set; }

        public string? Description { get; set; }



        public Domain.Model.AssessmentRequest ToDomainModel(IEnumerable<Phase> phases, IEnumerable<AssessmentType> assessmentTypes)
        {
            // TODO: Error handling if invalid/unrecognised phase or assessment type
            return new Domain.Model.AssessmentRequest()
            {
                Name = Name,
                Description = Description,
                PhaseConcluding = phases.FirstOrDefault(p => p.Id.ToString() == SelectedPhaseConcludingId),
                AssessmentType = assessmentTypes.FirstOrDefault(a => a.Id.ToString() == SelectedAssessmentTypeId),
                PhaseStartDate = PhaseStartDate,
                PhaseEndDate = PhaseEndDate,
            };
        }
    }
}
