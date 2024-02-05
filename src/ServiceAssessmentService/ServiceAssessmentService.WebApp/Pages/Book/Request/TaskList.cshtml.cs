using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request;

public class TaskListModel : PageModel
{

    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<TaskListModel> _logger;

    public TaskListModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<TaskListModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public AssessmentRequest? AssessmentRequest { get; set; }

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
