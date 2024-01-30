using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class EditModel : PageModel
{
    private readonly PhaseRepository _phaseRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(PhaseRepository phaseRepository, ILogger<EditModel> logger)
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

    public async Task<IActionResult> OnPost(Guid id, Phase newPhase)
    {
        Phase = await _phaseRepository.GetPhaseByIdAsync(id);
        if (Phase is null)
        {
            return NotFound($"Phase with ID {id} not found - cannot edit a phase which does not exist.");
        }

        await _phaseRepository.UpdatePhaseAsync(newPhase);
        return RedirectToPage("List");
    }
}
