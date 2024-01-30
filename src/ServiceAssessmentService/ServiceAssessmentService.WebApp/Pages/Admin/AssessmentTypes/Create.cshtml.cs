using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class CreateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<CreateModel> _logger;

    [BindProperty]
    public AssessmentType? AssessmentType { get; set; }

    public CreateModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<CreateModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        AssessmentType = new AssessmentType();
        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, AssessmentType newAssessmentType)
    {
        AssessmentType = newAssessmentType;

        var result = await _assessmentRequestRepository.CreateAssessmentTypeAsync(newAssessmentType);

        if (result is null)
        {
            _logger.LogWarning("Failed to create assessmentType {AssessmentType}", newAssessmentType);
            return Page();
        }
        else
        {
            return RedirectToPage("View", new { Id = result.Id });
        }
    }



}
