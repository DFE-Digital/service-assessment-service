using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceAssessmentService.Data;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class CreateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<CreateModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    [BindProperty] public NewAssessmentRequestSubmitModel? AssessmentRequestPageModel { get; set; }

    public void OnGet()
    {
        // If null, initialise an empty request model (this models the HTTP/form values, later to be mapped into a domain model)
        AssessmentRequestPageModel ??= new NewAssessmentRequestSubmitModel();
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


        var assessmentRequest = AssessmentRequestPageModel.ToDomainModel();
        assessmentRequest.Id = Guid.NewGuid();

        await _assessmentRequestRepository.CreateAsync(assessmentRequest);

        return RedirectToPage(nameof(View), new { assessmentRequest.Id });
    }


    public class NewAssessmentRequestSubmitModel
    {
        public string Name { get; set; } = string.Empty;

        public string PhaseConcluding { get; set; } = string.Empty;

        public string AssessmentType { get; set; } = string.Empty;

        public DateOnly? PhaseStartDate { get; set; }

        public DateOnly? PhaseEndDate { get; set; }

        public string? Description { get; set; }


        // to domain model

        public Domain.Model.AssessmentRequest ToDomainModel()
        {
            return new Domain.Model.AssessmentRequest()
            {
                Name = Name,
                Description = Description,
                PhaseConcluding = PhaseConcluding,
                AssessmentType = AssessmentType,
                PhaseStartDate = PhaseStartDate,
                PhaseEndDate = PhaseEndDate,
            };
        }
    }
}
