using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class SeedModel : PageModel
{
    private readonly PhaseRepository _phaseRepository;
    private readonly ILogger<SeedModel> _logger;

    public SeedModel(PhaseRepository phaseRepository, ILogger<SeedModel> logger)
    {
        _phaseRepository = phaseRepository;
        _logger = logger;
    }

    [BindProperty]
    public IEnumerable<Phase> Phases { get; set; } = new List<Phase>();

    public async Task<IActionResult> OnGet()
    {
        Phases = await _phaseRepository.GetPhasesAsync();
        return new PageResult();
    }

    public async Task<IActionResult> OnPost()
    {
        await _phaseRepository.SeedPhasesAsync();

        return RedirectToPage("/Admin/Phases/List");
    }
}
