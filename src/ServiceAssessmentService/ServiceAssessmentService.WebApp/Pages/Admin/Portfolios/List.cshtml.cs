using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class ListModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<ListModel> _logger;

    public ListModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<ListModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public IEnumerable<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    
    public async Task<IActionResult> OnGet()
    {
        Portfolios = await _assessmentRequestRepository.GetPortfoliosAsync();
        return new PageResult();
    }
}
