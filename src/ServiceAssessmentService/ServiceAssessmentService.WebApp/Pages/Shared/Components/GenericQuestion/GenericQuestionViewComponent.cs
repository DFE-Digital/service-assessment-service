using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

public class GenericQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(GenericQuestionHtmlModel question)
    {
        return View(question);
    }


    public class GenericQuestionHtmlModel
    {
        private readonly Question _question;

        public GenericQuestionHtmlModel(Question question)
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
                    Domain.Model.Questions.SimpleTextQuestion q => q.Answer,
                    _ => null,
                };
            }
            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case Domain.Model.Questions.SimpleTextQuestion q:
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
                Domain.Model.Questions.LongTextQuestion q => q.Answer,
                _ => null,
            };
            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case Domain.Model.Questions.LongTextQuestion q:
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
                Domain.Model.Questions.DateOnlyQuestion q => q.Answer,
                _ => null,
            };

            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case Domain.Model.Questions.DateOnlyQuestion q:
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
            Domain.Model.Questions.RadioQuestion q => q.Options.Select(o => new RadioOptionHtmlModel(o, q.Answer == o,
                o.NestedQuestion is null ? null : new GenericQuestionHtmlModel(o.NestedQuestion))),
            _ => Enumerable.Empty<RadioOptionHtmlModel>(),
        };

        public string? RadioAnswer
        {
            get => _question switch
            {
                Domain.Model.Questions.RadioQuestion q => q.Answer?.Value,
                _ => null,
            };

            set
            {
                if (_question == null)
                    throw new InvalidOperationException("Question cannot be null.");

                switch (_question)
                {
                    case Domain.Model.Questions.RadioQuestion q:
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
            private readonly Domain.Model.Questions.RadioQuestion.RadioOption _option;
            private readonly bool _isSelected;
            private readonly GenericQuestionHtmlModel? _nestedQuestion;

            public RadioOptionHtmlModel(Domain.Model.Questions.RadioQuestion.RadioOption option, bool isSelected,
                GenericQuestionHtmlModel? nestedQuestion)
            {
                _option = option;
                _isSelected = isSelected;
                _nestedQuestion = nestedQuestion;
            }

            public string DisplayTitle => _option.DisplayTitle;
            public string Value => _option.Value;
            public bool IsSelected => _isSelected;

            public GenericQuestionHtmlModel? NestedQuestion => _nestedQuestion;
        }
    }
}
