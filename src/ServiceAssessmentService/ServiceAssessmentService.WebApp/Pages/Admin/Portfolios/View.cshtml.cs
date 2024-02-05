using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class ViewModel : PageModel
{
    private readonly PortfolioRepository _portfolioRepository;
    private readonly ILogger<ViewModel> _logger;

    public ViewModel(PortfolioRepository portfolioRepository, ILogger<ViewModel> logger)
    {
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    [BindProperty]
    public Portfolio? Portfolio { get; set; }

    public async Task<IActionResult> OnGet(Guid id)
    {
        Portfolio = await _portfolioRepository.GetPortfolioByIdAsync(id);
        if (Portfolio is null)
        {
            return NotFound($"Portfolio with ID {id} not found");
        }

        return new PageResult();
    }
}
