using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class ListModel : PageModel
{
    private readonly PortfolioRepository _portfolioRepository;
    private readonly ILogger<ListModel> _logger;

    public ListModel(PortfolioRepository portfolioRepository, ILogger<ListModel> logger)
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
}
