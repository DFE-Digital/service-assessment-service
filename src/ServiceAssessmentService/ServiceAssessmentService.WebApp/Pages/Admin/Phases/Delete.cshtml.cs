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
    public Phase Phase { get; set; } = null!; // Must initialise within the OnGet/OnPost/etc handlers.

    public async Task<IActionResult> OnGet(Guid id)
    {
        var phase = await _phaseRepository.GetPhaseByIdAsync(id);
        if (phase is null)
        {
            _logger.LogWarning("Attempting to view Phase with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to view Phase with ID {id}, but it is not recognised");
        }

        Phase = phase;

        return new PageResult();
    }

    public async Task<IActionResult> OnPost(Guid id)
    {
        var phase = await _phaseRepository.GetPhaseByIdAsync(id);
        if (phase is null)
        {
            _logger.LogWarning("Attempting to delete Phase with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to delete Phase with ID {id}, but it is not recognised");
        }

        await _phaseRepository.DeletePhaseAsync(id);

        return RedirectToPage("List");
    }
}
