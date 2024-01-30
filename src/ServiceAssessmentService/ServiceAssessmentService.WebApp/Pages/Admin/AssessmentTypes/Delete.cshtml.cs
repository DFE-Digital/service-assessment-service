using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class DeleteModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<DeleteModel> _logger;

    [BindProperty]
    public AssessmentType? AssessmentType { get; set; }

    public DeleteModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<DeleteModel> logger)
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
    
    public async Task<IActionResult> OnPost(Guid id)
    {
        AssessmentType = await _assessmentRequestRepository.GetAssessmentTypeByIdAsync(id);
        if (AssessmentType is null)
        {
            return NotFound($"AssessmentType with ID {id} not found");
        }
        
        await _assessmentRequestRepository.DeleteAssessmentTypeAsync(id);
        return RedirectToPage("List");
    }
}
