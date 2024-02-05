using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class SeedModel : PageModel
{
    private readonly PortfolioRepository _portfolioRepository;
    private readonly ILogger<SeedModel> _logger;

    public SeedModel(PortfolioRepository portfolioRepository, ILogger<SeedModel> logger)
    {
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    [BindProperty]
    public IEnumerable<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

    public async Task<IActionResult> OnGet()
    {
        Portfolios = await _portfolioRepository.GetPortfoliosAsync();
        return new PageResult();
    }

    public async Task<IActionResult> OnPost()
    {
        await _portfolioRepository.SeedPortfoliosAsync();

        return RedirectToPage("/Admin/Portfolios/List");
    }
}
