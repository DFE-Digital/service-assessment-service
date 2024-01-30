using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class ViewModel : PageModel
{
    private readonly PhaseRepository _phaseRepository;
    private readonly ILogger<ViewModel> _logger;

    public ViewModel(PhaseRepository phaseRepository, ILogger<ViewModel> logger)
    {
        _phaseRepository = phaseRepository;
        _logger = logger;
    }

    [BindProperty]
    public Phase? Phase { get; set; }

    public async Task<IActionResult> OnGet(Guid id)
    {
        Phase = await _phaseRepository.GetPhaseByIdAsync(id);
        if (Phase is null)
        {
            return NotFound($"Phase with ID {id} not found");
        }

        return new PageResult();
    }
}
