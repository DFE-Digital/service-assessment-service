using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class EditModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<EditModel> _logger;

    [BindProperty]
    public Portfolio? Portfolio { get; set; }

    public EditModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<EditModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }
    
    public async Task<IActionResult> OnGet(Guid id)
    {
        Portfolio = await _assessmentRequestRepository.GetPortfolioByIdAsync(id);
        if (Portfolio is null)
        {
            return NotFound($"Portfolio with ID {id} not found");
        }
        
        return new PageResult();
    }
    
    public async Task<IActionResult> OnPost(Guid id, Portfolio newPortfolio)
    {
        Portfolio = await _assessmentRequestRepository.GetPortfolioByIdAsync(id);
        if (Portfolio is null)
        {
            return NotFound($"Portfolio with ID {id} not found - cannot edit a portfolio which does not exist.");
        }
        
        await _assessmentRequestRepository.UpdatePortfolioAsync(newPortfolio);
        return RedirectToPage("List");
    }
}
