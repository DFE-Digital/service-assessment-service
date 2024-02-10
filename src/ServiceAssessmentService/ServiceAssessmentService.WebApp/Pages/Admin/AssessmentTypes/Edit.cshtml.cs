using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class EditModel : PageModel
{
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(AssessmentTypeRepository assessmentTypeRepository, ILogger<EditModel> logger)
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

    public async Task<IActionResult> OnPost(Guid id, AssessmentType newAssessmentType)
    {
        var assessmentType = await _assessmentTypeRepository.GetAssessmentTypeByIdAsync(id);
        if (assessmentType is null)
        {
            _logger.LogWarning("Attempting to edit AssessmentType with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to edit AssessmentType with ID {id}, but it is not recognised");
        }

        await _assessmentTypeRepository.UpdateAssessmentTypeAsync(newAssessmentType);

        return RedirectToPage("List");
    }
}
