using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class CreateModel : PageModel
{
    private readonly PhaseRepository _phaseRepository;

    private readonly ILogger<CreateModel> _logger;

    public CreateModel(PhaseRepository phaseRepository, ILogger<CreateModel> logger
    )
    {
        _phaseRepository = phaseRepository;
        _logger = logger;
    }


    [BindProperty]
    public Phase? Phase { get; set; }


    public IActionResult OnGet()
    {
        Phase = new Phase();
        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, Phase newPhase)
    {
        Phase = newPhase;

        var result = await _phaseRepository.CreatePhaseAsync(newPhase);
        if (result is null)
        {
            _logger.LogWarning("Failed to create phase {Phase}", newPhase);
            return Page();
        }

        return RedirectToPage("View", new { Id = result.Id });
    }
}
