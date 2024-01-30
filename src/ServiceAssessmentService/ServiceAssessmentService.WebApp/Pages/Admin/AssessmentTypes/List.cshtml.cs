using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.AssessmentTypes;

public class ListModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<ListModel> _logger;

    public ListModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<ListModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public IEnumerable<AssessmentType> AssessmentTypes { get; set; } = new List<AssessmentType>();
    
    public async Task<IActionResult> OnGet()
    {
        AssessmentTypes = await _assessmentRequestRepository.GetAssessmentTypesAsync();
        return new PageResult();
    }
}
