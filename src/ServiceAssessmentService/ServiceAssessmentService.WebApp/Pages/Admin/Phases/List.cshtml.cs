using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Phases;

public class ListModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<ListModel> _logger;

    public ListModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<ListModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public IEnumerable<Phase> Phases { get; set; } = new List<Phase>();
    
    public async Task<IActionResult> OnGet()
    {
        Phases = await _assessmentRequestRepository.GetPhasesAsync();
        return new PageResult();
    }
}
