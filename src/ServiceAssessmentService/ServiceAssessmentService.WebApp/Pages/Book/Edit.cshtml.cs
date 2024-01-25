using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceAssessmentService.Application;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.WebApp.Pages.Book;

public class EditModel : PageModel
{
    [BindProperty] public EditAssessmentRequestHtmlModel AssessmentRequestPageModel { get; set; }

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

        return RedirectToPage("/Book/View", new {id});
    }

    public class EditAssessmentRequestHtmlModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhaseConcluding { get; set; } = string.Empty;

        public string AssessmentType { get; set; } = string.Empty;

        public DateOnly? PhaseStartDate { get; set; }

        public DateOnly? PhaseEndDate { get; set; }

        public string? Description { get; set; }


        public Domain.Model.AssessmentRequest ToDomainModel()
        {
            return new Domain.Model.AssessmentRequest()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                PhaseConcluding = ProjectPhase.FromName(PhaseConcluding),
                AssessmentType = ServiceAssessmentService.Domain.Model.AssessmentType.FromName(AssessmentType),
                PhaseStartDate = PhaseStartDate,
                PhaseEndDate = PhaseEndDate,
                // Questions =  Questions.Select(q => q.ToDomainModel()),
            };
        }

        public static EditAssessmentRequestHtmlModel FromDomainModel(AssessmentRequest request)
        {
            return new EditAssessmentRequestHtmlModel()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                PhaseConcluding = request.PhaseConcluding?.Name ?? string.Empty,
                AssessmentType = request.AssessmentType?.Name ?? string.Empty,
                PhaseStartDate = request.PhaseStartDate,
                PhaseEndDate = request.PhaseEndDate,
                Questions = request.Questions.Select(q => new QuestionHtmlModel(q)),
            };
        }

        public IEnumerable<QuestionHtmlModel> Questions { get; set; } = Enumerable.Empty<QuestionHtmlModel>();
    }

    public class QuestionHtmlModel
    {
        private readonly Question _question;

        public QuestionHtmlModel(Question question)
        {
            _question = question;
        }

        public string Title
        {
            get => _question.Title;
            set => _question.Title = value;
        }

        public string HintText
        {
            get => _question.HintText;
            set => _question.HintText = value;
        }

        public QuestionType Type
        {
            get => _question.Type;
            set => _question.Type = value;
        }

        public string? SimpleTextAnswer
        {
            get
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                return _question switch
                {
                    SimpleTextQuestion q => q.Answer,
                    _ => null,
                };
            }
            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case SimpleTextQuestion q:
                        q.Answer = value;
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"{_question.GetType().Name} does not support SimpleTextAnswer.");
                }
            }
        }

        public string? LongTextAnswer
        {
            get => _question switch
            {
                LongTextQuestion q => q.Answer,
                _ => null,
            };
            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case LongTextQuestion q:
                        q.Answer = value;
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"{_question.GetType().Name} does not support LongTextAnswer.");
                }
            }
        }

        public DateOnly? DateOnlyAnswer
        {
            get => _question switch
            {
                DateOnlyQuestion q => q.Answer,
                _ => null,
            };

            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case DateOnlyQuestion q:
                        q.Answer = value;
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"{_question.GetType().Name} does not support DateOnlyAnswer.");
                }
            }
        }

        public IEnumerable<RadioOptionHtmlModel> RadioOptions => _question switch
        {
            RadioQuestion q => q.Options.Select(o => new RadioOptionHtmlModel(o, q.Answer == o,
                o.NestedQuestion is null ? null : new QuestionHtmlModel(o.NestedQuestion))),
            _ => Enumerable.Empty<RadioOptionHtmlModel>(),
        };

        public string? RadioAnswer
        {
            get => _question switch
            {
                RadioQuestion q => q.Answer?.Value,
                _ => null,
            };

            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case RadioQuestion q:
                        q.Answer = q.Options.FirstOrDefault(o => o.Value == value);
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"{_question.GetType().Name} does not support RadioAnswer.");
                }
            }
        }

        public class RadioOptionHtmlModel
        {
            private readonly RadioQuestion.RadioOption _option;
            private readonly bool _isSelected;
            private readonly QuestionHtmlModel? _nestedQuestion;

            public RadioOptionHtmlModel(RadioQuestion.RadioOption option, bool isSelected,
                QuestionHtmlModel? nestedQuestion)
            {
                _option = option;
                _isSelected = isSelected;
                _nestedQuestion = nestedQuestion;
            }

            public string DisplayTitle => _option.DisplayTitle;
            public string Value => _option.Value;
            public bool IsSelected => _isSelected;

            public QuestionHtmlModel? NestedQuestion => _nestedQuestion;
        }
    }
}
