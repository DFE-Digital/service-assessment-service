using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class ViewModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<ViewModel> _logger;

    public Phase? Phase { get; set; }

    public ViewModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<ViewModel> logger)
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
}
