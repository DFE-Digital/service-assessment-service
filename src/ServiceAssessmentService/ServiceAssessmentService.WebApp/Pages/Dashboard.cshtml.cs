using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.WebApp.Pages.Book;

namespace ServiceAssessmentService.WebApp.Pages;

public class DashboardModel : PageModel
{
    public IEnumerable<AssessmentRequest> AllAssessments;


    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<DashboardModel> _logger;

    public DashboardModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<DashboardModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        AllAssessments = await _assessmentRequestRepository.GetAllAssessmentRequests();

        return Page();
    }

}
