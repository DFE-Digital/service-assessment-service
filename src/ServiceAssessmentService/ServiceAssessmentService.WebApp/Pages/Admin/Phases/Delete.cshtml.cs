using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class DeleteModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<DeleteModel> _logger;

    [BindProperty]
    public Phase? Phase { get; set; }

    public DeleteModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<DeleteModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }
    
    public async Task<IActionResult> OnGet(Guid id)
    {
        Phase = await _assessmentRequestRepository.GetPhaseByIdAsync(id);
        if (Phase is null)
        {
            return NotFound($"Phase with ID {id} not found");
        }
        
        return new PageResult();
    }
    
    public async Task<IActionResult> OnPost(Guid id)
    {
        Phase = await _assessmentRequestRepository.GetPhaseByIdAsync(id);
        if (Phase is null)
        {
            return NotFound($"Phase with ID {id} not found");
        }
        
        await _assessmentRequestRepository.DeletePhaseAsync(id);
        return RedirectToPage("List");
    }
}
