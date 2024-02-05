using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class CreateModel : PageModel
{
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(AssessmentTypeRepository assessmentTypeRepository, ILogger<CreateModel> logger)
    {
        _assessmentTypeRepository = assessmentTypeRepository;
        _logger = logger;
    }

    [BindProperty]
    public AssessmentType? AssessmentType { get; set; }

    public IActionResult OnGet()
    {
        AssessmentType = new AssessmentType();
        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, AssessmentType newAssessmentType)
    {
        AssessmentType = newAssessmentType;

        var result = await _assessmentTypeRepository.CreateAssessmentTypeAsync(newAssessmentType);
        if (result is null)
        {
            _logger.LogWarning("Failed to create assessmentType {AssessmentType}", newAssessmentType);
            return Page();
        }

        return RedirectToPage("View", new { Id = result.Id });
    }
}
