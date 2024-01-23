using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class ViewModel : PageModel
{
    public AssessmentRequest? AssessmentRequest { get; set; }

    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<ViewModel> _logger;

    public ViewModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<ViewModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {
        AssessmentRequest = await _assessmentRequestRepository.GetByIdAsync(id);

        if (AssessmentRequest is null)
        {
            _logger.LogInformation("Attempted to view assessment request with ID {Id}, but it was not found", id);
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        }

        return Page();
    }
}
