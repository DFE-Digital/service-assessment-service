using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class ListModel : PageModel
{
    private readonly AssessmentTypeRepository _assessmentTypeRepository;
    private readonly ILogger<ListModel> _logger;

    public ListModel(AssessmentTypeRepository assessmentTypeRepository, ILogger<ListModel> logger)
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
}
