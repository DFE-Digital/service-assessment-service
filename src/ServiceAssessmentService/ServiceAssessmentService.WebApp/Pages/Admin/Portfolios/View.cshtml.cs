using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Admin.Portfolios;

public class ViewModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<ViewModel> _logger;

    public Portfolio? Portfolio { get; set; }

    public ViewModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<ViewModel> logger)
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
}
