using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.DateOnlyQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.LongTextQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.RadioQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.SimpleTextQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class EditModel : PageModel
{
    [BindProperty]
    public EditAssessmentRequestHtmlModel AssessmentRequestPageModel { get; set; }

    public IEnumerable<ProjectPhase> AllPhases { get; set; }
    public IEnumerable<ServiceAssessmentService.Domain.Model.AssessmentType> AllAssessmentTypes { get; set; }

    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(AssessmentRequestRepository assessmentRequestRepository, ILogger<EditModel> logger)
    {
        _assessmentRequestRepository = assessmentRequestRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var req = await _assessmentRequestRepository.GetByIdAsync(id);
        if (req == null)
        {
            _logger.LogInformation("Attempted to edit assessment request with ID {Id}, but it was not found", id);
            return NotFound();
        }

        AllPhases = ProjectPhase.Sequence;
        AllAssessmentTypes = ServiceAssessmentService.Domain.Model.AssessmentType.All;

        AssessmentRequestPageModel = EditAssessmentRequestHtmlModel.FromDomainModel(req);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        AllPhases = ProjectPhase.Sequence;
        AllAssessmentTypes = ServiceAssessmentService.Domain.Model.AssessmentType.All;

        // Update values from submission
        var req = AssessmentRequestPageModel.ToDomainModel();
        req.Id = id;

        await _assessmentRequestRepository.UpdateAsync(req);

        return RedirectToPage("/Book/View", new { id });
    }

    public class EditAssessmentRequestHtmlModel
    {
        public Guid Id { get; set; }


        public Domain.Model.AssessmentRequest ToDomainModel()
        {
            var assessmentRequest = new Domain.Model.AssessmentRequest()
            {
                Id = Id,
                Questions = Questions.Select<GenericQuestionViewComponent.GenericQuestionHtmlModel, Domain.Model.Questions.Question>(htmlQuestion =>
                {
                    switch (htmlQuestion)
                    {
                        case SimpleTextQuestionViewComponent.SimpleTextQuestionHtmlModel sq:
                            return sq.ToDomain();
                        case LongTextQuestionViewComponent.LongTextQuestionHtmlModel lq:
                            return lq.ToDomain();
                        case DateOnlyQuestionViewComponent.DateOnlyQuestionHtmlModel dq:
                            return dq.ToDomain();
                        case RadioQuestionViewComponent.RadioQuestionHtmlModel rq:
                            return rq.ToDomain();
                        default:
                            throw new InvalidOperationException($"Unknown question type {htmlQuestion.GetType().Name}");
                    }
                }),
            };

            return assessmentRequest;
        }

        public static EditAssessmentRequestHtmlModel FromDomainModel(AssessmentRequest request)
        {
            return new EditAssessmentRequestHtmlModel()
            {
                Id = request.Id,
                Questions = request.Questions.Select<Domain.Model.Questions.Question, GenericQuestionViewComponent.GenericQuestionHtmlModel>(domainQuestion =>
                {
                    switch (domainQuestion)
                    {
                        case Domain.Model.Questions.SimpleTextQuestion sq:
                            return SimpleTextQuestionViewComponent.SimpleTextQuestionHtmlModel.FromDomainModel(sq);
                        case Domain.Model.Questions.LongTextQuestion lq:
                            return LongTextQuestionViewComponent.LongTextQuestionHtmlModel.FromDomainModel(lq);
                        case Domain.Model.Questions.DateOnlyQuestion dq:
                            return DateOnlyQuestionViewComponent.DateOnlyQuestionHtmlModel.FromDomainModel(dq);
                        case Domain.Model.Questions.RadioQuestion rq:
                            return RadioQuestionViewComponent.RadioQuestionHtmlModel.FromDomainModel(rq);
                        default:
                            throw new InvalidOperationException(
                                $"Unknown question type {domainQuestion.GetType().Name}");
                    }
                }),
            };
        }

        public IEnumerable<GenericQuestionViewComponent.GenericQuestionHtmlModel> Questions { get; set; } = Enumerable.Empty<GenericQuestionViewComponent.GenericQuestionHtmlModel>();
    }

}
