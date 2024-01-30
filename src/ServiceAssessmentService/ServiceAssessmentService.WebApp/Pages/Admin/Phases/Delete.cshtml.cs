using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class DeleteModel : PageModel
{
    private readonly PhaseRepository _phaseRepository;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(PhaseRepository phaseRepository, ILogger<DeleteModel> logger)
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

    public async Task<IActionResult> OnPost(Guid id)
    {
        Phase = await _phaseRepository.GetPhaseByIdAsync(id);
        if (Phase is null)
        {
            return NotFound($"Phase with ID {id} not found");
        }

        await _phaseRepository.DeletePhaseAsync(id);
        return RedirectToPage("List");
    }
}
