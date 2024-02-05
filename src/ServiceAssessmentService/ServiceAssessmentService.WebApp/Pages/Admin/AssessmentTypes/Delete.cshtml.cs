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

    public async Task<IActionResult> OnPost(Guid id)
    {
        AssessmentType = await _assessmentTypeRepository.GetAssessmentTypeByIdAsync(id);
        if (AssessmentType is null)
        {
            return NotFound($"AssessmentType with ID {id} not found");
        }

        await _assessmentTypeRepository.DeleteAssessmentTypeAsync(id);
        return RedirectToPage("List");
    }
}
