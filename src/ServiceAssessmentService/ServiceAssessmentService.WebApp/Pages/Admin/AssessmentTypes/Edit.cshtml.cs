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
    public AssessmentType? AssessmentType { get; set; }

    public async Task<IActionResult> OnGet(Guid id)
    {
        AssessmentType = await _assessmentTypeRepository.GetAssessmentTypeByIdAsync(id);
        if (AssessmentType is null)
        {
            return NotFound($"AssessmentType with ID {id} not found");
        }

        return new PageResult();
    }

    public async Task<IActionResult> OnPost(Guid id, AssessmentType newAssessmentType)
    {
        AssessmentType = await _assessmentTypeRepository.GetAssessmentTypeByIdAsync(id);
        if (AssessmentType is null)
        {
            return NotFound($"AssessmentType with ID {id} not found - cannot edit an assessmentType which does not exist.");
        }

        await _assessmentTypeRepository.UpdateAssessmentTypeAsync(newAssessmentType);
        return RedirectToPage("List");
    }
}
