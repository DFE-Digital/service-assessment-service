using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class SeedModel : PageModel
{
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly ILogger<SeedModel> _logger;

    public SeedModel(AssessmentTypeRepository assessmentTypeRepository, ILogger<SeedModel> logger)
    {
        _assessmentTypeRepository = assessmentTypeRepository;
        _logger = logger;
    }

    [BindProperty]
    public IEnumerable<AssessmentType> AssessmentTypes { get; set; } = new List<AssessmentType>();

    public async Task<IActionResult> OnGet()
    {
        AssessmentTypes = await _assessmentTypeRepository.GetAssessmentTypesAsync();
        return new PageResult();
    }

    public async Task<IActionResult> OnPost()
    {
        await _assessmentTypeRepository.SeedAssessmentTypesAsync();

        return RedirectToPage("/Admin/AssessmentTypes/List");
    }
}
