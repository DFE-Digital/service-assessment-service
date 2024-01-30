using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class CreateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<CreateModel> _logger;

    [BindProperty]
    public Phase? Phase { get; set; }

    public CreateModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<CreateModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        Phase = new Phase();
        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, Phase newPhase)
    {
        Phase = newPhase;

        var result = await _assessmentRequestRepository.CreatePhaseAsync(newPhase);

        if (result is null)
        {
            _logger.LogWarning("Failed to create phase {Phase}", newPhase);
            return Page();
        }
        else
        {
            return RedirectToPage("View", new { Id = result.Id });
        }
    }



}
