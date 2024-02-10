using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class EditModel : PageModel
{
    private readonly PortfolioRepository _portfolioRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(PortfolioRepository portfolioRepository, ILogger<EditModel> logger)
    {
        _portfolioRepository = portfolioRepository;
        _logger = logger;
    }

    [BindProperty]
    public Portfolio Portfolio { get; set; } = null!; // Must initialise within the OnGet/OnPost/etc handlers.

    public async Task<IActionResult> OnGet(Guid id)
    {
        var portfolio = await _portfolioRepository.GetPortfolioByIdAsync(id);
        if (portfolio is null)
        {
            _logger.LogWarning("Attempting to view Portfolio with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to view Portfolio with ID {id}, but it is not recognised");
        }

        Portfolio = portfolio;

        return new PageResult();
    }

    public async Task<IActionResult> OnPost(Guid id, Portfolio newPortfolio)
    {
        var portfolio = await _portfolioRepository.GetPortfolioByIdAsync(id);
        if (portfolio is null)
        {
            _logger.LogWarning("Attempting to delete Portfolio with ID {Id}, but it is not recognised", id);
            return NotFound($"Attempting to delete Portfolio with ID {id}, but it is not recognised");
        }

        await _portfolioRepository.UpdatePortfolioAsync(newPortfolio);

        return RedirectToPage("List");
    }
}
