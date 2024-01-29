using System.Collections;
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
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<CreateModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    [BindProperty] public NewAssessmentRequestSubmitModel? AssessmentRequestPageModel { get; set; }

    public IEnumerable<ProjectPhase> AllPhases { get; set; }
    public IEnumerable<ServiceAssessmentService.Domain.Model.AssessmentType> AllAssessmentTypes { get; set; }

    public void OnGet()
    {
        // If null, initialise an empty request model (this models the HTTP/form values, later to be mapped into a domain model)
        AssessmentRequestPageModel ??= new NewAssessmentRequestSubmitModel();

        AllPhases = ProjectPhase.Sequence;
        AllAssessmentTypes = ServiceAssessmentService.Domain.Model.AssessmentType.All;
    }


    public async Task<IActionResult> OnPostAsync()
    {
        AllPhases = ProjectPhase.Sequence;
        AllAssessmentTypes = ServiceAssessmentService.Domain.Model.AssessmentType.All;

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
        //public Question
        // to domain model

        public Domain.Model.AssessmentRequest ToDomainModel()
        {
            var assessmentRequest = new Domain.Model.AssessmentRequest()
            {
                Id = Guid.NewGuid(),
                //Questions = Questions.Select(q => q.ToDomainModel()).ToList(),
                Questions = new List<Domain.Model.Questions.Question>(),
            };

            return assessmentRequest;
        }
    }
}
