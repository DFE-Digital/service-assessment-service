using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class SeedModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<SeedModel> _logger;

    public IEnumerable<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

    public SeedModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<SeedModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        Portfolios = await _assessmentRequestRepository.GetPortfoliosAsync();
        return new PageResult();
    }

    public async Task<IActionResult> OnPost()
    {
        await _assessmentRequestRepository.SeedPortfoliosAsync();

        return RedirectToPage("/Admin/Portfolios/List");
    }
}
