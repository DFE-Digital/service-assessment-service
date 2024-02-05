using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;

namespace ServiceAssessmentService.WebApp.Pages.Book.Request;

public class StartNewPageModel : PageModel
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<StartNewPageModel> _logger;

    public StartNewPageModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<StartNewPageModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }


    public async Task<IActionResult> OnPost()
    {
        _logger.LogInformation("Attempting to create a new assessment request");

        var blankRequest = new Domain.Model.AssessmentRequest()
        {
            Id = Guid.NewGuid(),
        };

        var assessmentRequest = await _assessmentRequestRepository.CreateAsync(blankRequest);
        _logger.LogInformation("Created new assessment request with ID {Id}", assessmentRequest.Id);

        return RedirectToPage("/Book/Request/Question/PhaseConcluding", new { assessmentRequest.Id });
    }
}
