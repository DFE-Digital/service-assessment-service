using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class EditModel : PageModel
{
    [BindProperty] public EditAssessmentRequestSubmitModel AssessmentRequestPageModel { get; set; }

    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly PhaseRepository _phaseRepository;
    private readonly PortfolioRepository _portfolioRepository;

    private readonly ILogger<EditModel> _logger;

    public EditModel(
        AssessmentRequestRepository assessmentRequestRepository
        , AssessmentTypeRepository assessmentTypeRepository
        , PhaseRepository phaseRepository
        , PortfolioRepository portfolioRepository
        , ILogger<EditModel> logger
    )
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _assessmentTypeRepository = assessmentTypeRepository;
        _phaseRepository = phaseRepository;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        AssessmentRequestPageModel = EditAssessmentRequestSubmitModel.FromDomainModel(req);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, EditAssessmentRequestSubmitModel newModel)
    {
        if (!ModelState.IsValid)
        {
            AssessmentRequestPageModel = newModel;
            return Page();
        }

        var phases = await _phaseRepository.GetPhasesAsync();
        var assessmentTypes = await _assessmentTypeRepository.GetAssessmentTypesAsync();

        // Update values from submission
        var req = AssessmentRequestPageModel.ToDomainModel(phases, assessmentTypes);
        req.Id = id;

        await _assessmentRequestRepository.UpdateAsync(req);

        return RedirectToPage("/Book/View", new { id });
    }

    public class EditAssessmentRequestSubmitModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhaseConcluding { get; set; } = string.Empty;

        public string AssessmentType { get; set; } = string.Empty;

        public DateOnly? PhaseStartDate { get; set; }

        public DateOnly? PhaseEndDate { get; set; }

        public string? Description { get; set; }


        public Domain.Model.AssessmentRequest ToDomainModel(IEnumerable<Phase> phases, IEnumerable<AssessmentType> assessmentTypes)
        {
            var phaseConcluding = phases.SingleOrDefault(p => p.Id.ToString() == PhaseConcluding);
            var assessmentType = assessmentTypes.SingleOrDefault(a => a.Id.ToString() == AssessmentType);

            return new Domain.Model.AssessmentRequest()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                PhaseConcluding = phaseConcluding,
                AssessmentType = assessmentType,
                PhaseStartDate = PhaseStartDate,
                PhaseEndDate = PhaseEndDate,
            };
        }

        public static EditAssessmentRequestSubmitModel FromDomainModel(AssessmentRequest request)
        {
            return new EditAssessmentRequestSubmitModel()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                PhaseConcluding = request.PhaseConcluding?.Id.ToString() ?? string.Empty,
                AssessmentType = request.AssessmentType?.Id.ToString() ?? string.Empty,
                PhaseStartDate = request.PhaseStartDate,
                PhaseEndDate = request.PhaseEndDate,
            };
        }
    }
}
