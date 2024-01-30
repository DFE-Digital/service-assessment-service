using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class EditModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<EditModel> _logger;

    [BindProperty]
    public AssessmentType? AssessmentType { get; set; }

    public EditModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<EditModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }
    
    public async Task<IActionResult> OnGet(Guid id)
    {
        AssessmentType = await _assessmentRequestRepository.GetAssessmentTypeByIdAsync(id);
        if (AssessmentType is null)
        {
            return NotFound($"AssessmentType with ID {id} not found");
        }
        
        return new PageResult();
    }
    
    public async Task<IActionResult> OnPost(Guid id, AssessmentType newAssessmentType)
    {
        AssessmentType = await _assessmentRequestRepository.GetAssessmentTypeByIdAsync(id);
        if (AssessmentType is null)
        {
            return NotFound($"AssessmentType with ID {id} not found - cannot edit an assessmentType which does not exist.");
        }
        
        await _assessmentRequestRepository.UpdateAssessmentTypeAsync(newAssessmentType);
        return RedirectToPage("List");
    }
}
