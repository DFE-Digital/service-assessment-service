using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class CreateModel : PageModel
{
    private readonly PortfolioRepository _portfolioRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(PortfolioRepository portfolioRepository, ILogger<CreateModel> logger)
    {
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    [BindProperty]
    public Portfolio Portfolio { get; set; } = null!; // Must initialise within the OnGet/OnPost/etc handlers.

    public IActionResult OnGet()
    {
        Portfolio = new Portfolio();
        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, Portfolio newPortfolio)
    {
        Portfolio = newPortfolio;

        var result = await _portfolioRepository.CreatePortfolioAsync(newPortfolio);
        if (result is null)
        {
            _logger.LogWarning("Failed to create portfolio {Portfolio}", newPortfolio);
            return Page();
        }

        return RedirectToPage("View", new { Id = result.Id });
    }
}
