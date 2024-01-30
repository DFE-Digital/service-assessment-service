using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class SeedModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<SeedModel> _logger;

    public IEnumerable<Phase> Phases { get; set; } = new List<Phase>();

    public SeedModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<SeedModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        Phases = await _assessmentRequestRepository.GetPhasesAsync();
        return new PageResult();
    }

    public async Task<IActionResult> OnPost()
    {
        await _assessmentRequestRepository.SeedPhasesAsync();

        return RedirectToPage("/Admin/Phases/List");
    }
}
