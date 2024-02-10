using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class DeleteModel : PageModel
{
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(AssessmentTypeRepository assessmentTypeRepository, ILogger<DeleteModel> logger)
    {
        _assessmentTypeRepository = assessmentTypeRepository;
        _logger = logger;
    }

    [BindProperty]
    public AssessmentType AssessmentType { get; set; } = null!; // Must initialise within the OnGet/OnPost/etc handlers.

    public async Task<IActionResult> OnGet(Guid id)
    {
        var assessmentType = await _assessmentTypeRepository.GetAssessmentTypeByIdAsync(id);
        if (assessmentType is null)
        {
            _logger.LogWarning("Attempting to view AssessmentType with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to view AssessmentType with ID {id}, but it is not recognised");
        }

        AssessmentType = assessmentType;

        return new PageResult();
    }

    public async Task<IActionResult> OnPost(Guid id)
    {
        var assessmentType = await _assessmentTypeRepository.GetAssessmentTypeByIdAsync(id);
        if (assessmentType is null)
        {
            _logger.LogWarning("Attempting to delete AssessmentType with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to delete AssessmentType with ID {id}, but it is not recognised");
        }

        await _assessmentTypeRepository.DeleteAssessmentTypeAsync(id);

        return RedirectToPage("List");
    }
}
