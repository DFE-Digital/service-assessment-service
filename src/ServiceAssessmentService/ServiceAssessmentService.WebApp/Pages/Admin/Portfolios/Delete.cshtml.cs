using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class DeleteModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<DeleteModel> _logger;

    [BindProperty]
    public Portfolio? Portfolio { get; set; }

    public DeleteModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<DeleteModel> logger)
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
    
    public async Task<IActionResult> OnPost(Guid id)
    {
        Portfolio = await _assessmentRequestRepository.GetPortfolioByIdAsync(id);
        if (Portfolio is null)
        {
            return NotFound($"Portfolio with ID {id} not found");
        }
        
        await _assessmentRequestRepository.DeletePortfolioAsync(id);
        return RedirectToPage("List");
    }
}
