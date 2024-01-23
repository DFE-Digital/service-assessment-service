using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class ListModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    public IEnumerable<AssessmentRequest> AssessmentRequests { get; private set; } = new List<AssessmentRequest>();

    public ListModel(AssessmentRequestRepository assessmentRequestRepository)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        AssessmentRequests = await _assessmentRequestRepository.GetAllAssessmentRequests();
        return new PageResult();
    }
}
