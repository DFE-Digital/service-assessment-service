using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.WebApp.Pages.Book
{
    public class DeletePageModel : PageModel
    {
        public AssessmentRequest AssessmentRequest { get; set; }

        private readonly AssessmentRequestRepository _assessmentRequestRepository;
        private readonly ILogger<DeletePageModel> _logger;

        public DeletePageModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<DeletePageModel> logger)
        {
            _assessmentRequestRepository = assessmentRequestRepository;
            _logger = logger;
        }


        public async Task<IActionResult> OnGet(Guid id)
        {
            _logger.LogInformation("Attempting to delete assessment request with ID {Id}", id);
            var req = await _assessmentRequestRepository.GetByIdAsync(id);
            if (req == null)
            {
                return NotFound();
            }

            AssessmentRequest = req;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var assessmentRequest = await _assessmentRequestRepository.DeleteAsync(id);
            if (assessmentRequest == null)
            {
                _logger.LogInformation("Attempted to delete assessment request with ID {Id}, but it was not found", id);
                return NotFound();
            }

            return RedirectToPage("/Book");
        }
    }
}
