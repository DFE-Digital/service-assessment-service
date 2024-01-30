using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class SeedModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<SeedModel> _logger;

    public IEnumerable<AssessmentType> AssessmentTypes { get; set; } = new List<AssessmentType>();

    public SeedModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<SeedModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        AssessmentTypes = await _assessmentRequestRepository.GetAssessmentTypesAsync();
        return new PageResult();
    }

    public async Task<IActionResult> OnPost()
    {
        await _assessmentRequestRepository.SeedAssessmentTypesAsync();

        return RedirectToPage("/Admin/AssessmentTypes/List");
    }
}
