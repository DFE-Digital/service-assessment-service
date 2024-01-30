using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class EditModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<EditModel> _logger;

    [BindProperty]
    public Phase? Phase { get; set; }

    public EditModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<EditModel> logger)
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
    
    public async Task<IActionResult> OnPost(Guid id, Phase newPhase)
    {
        Phase = await _assessmentRequestRepository.GetPhaseByIdAsync(id);
        if (Phase is null)
        {
            return NotFound($"Phase with ID {id} not found - cannot edit a phase which does not exist.");
        }
        
        await _assessmentRequestRepository.UpdatePhaseAsync(newPhase);
        return RedirectToPage("List");
    }
}
