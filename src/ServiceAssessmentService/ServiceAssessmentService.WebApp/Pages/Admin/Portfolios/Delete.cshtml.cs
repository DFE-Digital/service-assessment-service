using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class DeleteModel : PageModel
{
    private readonly PortfolioRepository _portfolioRepository;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(PortfolioRepository portfolioRepository, ILogger<DeleteModel> logger)
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

    public async Task<IActionResult> OnPost(Guid id)
    {
        Portfolio = await _portfolioRepository.GetPortfolioByIdAsync(id);
        if (Portfolio is null)
        {
            return NotFound($"Portfolio with ID {id} not found");
        }

        await _portfolioRepository.DeletePortfolioAsync(id);
        return RedirectToPage("List");
    }
}
