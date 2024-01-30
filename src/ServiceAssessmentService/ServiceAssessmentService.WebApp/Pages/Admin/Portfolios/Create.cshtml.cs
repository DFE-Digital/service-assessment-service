using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class CreateModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<CreateModel> _logger;

    [BindProperty]
    public Portfolio? Portfolio { get; set; }

    public CreateModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<CreateModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        Portfolio = new Portfolio();
        return Page();
    }

    public async Task<IActionResult> OnPost(Guid id, Portfolio newPortfolio)
    {
        Portfolio = newPortfolio;

        var result = await _assessmentRequestRepository.CreatePortfolioAsync(newPortfolio);

        if (result is null)
        {
            _logger.LogWarning("Failed to create portfolio {Portfolio}", newPortfolio);
            return Page();
        }
        else
        {
            return RedirectToPage("View", new { Id = result.Id });
        }
    }



}
