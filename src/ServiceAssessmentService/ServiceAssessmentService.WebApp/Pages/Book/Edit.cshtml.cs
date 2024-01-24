using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class EditModel : PageModel
{
    [BindProperty] public EditAssessmentRequestSubmitModel AssessmentRequestPageModel { get; set; }

    public IEnumerable<ProjectPhase> AllPhases { get; set; }
    public IEnumerable<ServiceAssessmentService.Domain.Model.AssessmentType> AllAssessmentTypes { get; set; }

    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<EditModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
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
        
        AllPhases = ProjectPhase.Sequence;
        AllAssessmentTypes = ServiceAssessmentService.Domain.Model.AssessmentType.All;

        AssessmentRequestPageModel = EditAssessmentRequestSubmitModel.FromDomainModel(req);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        AllPhases = ProjectPhase.Sequence;
        AllAssessmentTypes = ServiceAssessmentService.Domain.Model.AssessmentType.All;

        // Update values from submission
        var req = AssessmentRequestPageModel.ToDomainModel();
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


        public Domain.Model.AssessmentRequest ToDomainModel()
        {
            return new Domain.Model.AssessmentRequest()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                PhaseConcluding = ProjectPhase.FromName(PhaseConcluding),
                AssessmentType = ServiceAssessmentService.Domain.Model.AssessmentType.FromName(AssessmentType),
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
                PhaseConcluding = request.PhaseConcluding?.Name ?? string.Empty,
                AssessmentType = request.AssessmentType?.Name ?? string.Empty,
                PhaseStartDate = request.PhaseStartDate,
                PhaseEndDate = request.PhaseEndDate,
            };
        }
    }
}
