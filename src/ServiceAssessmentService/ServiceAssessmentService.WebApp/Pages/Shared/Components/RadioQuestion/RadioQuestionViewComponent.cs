using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.Domain.Model.Questions;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.DateOnlyQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.LongTextQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.SimpleTextQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.RadioQuestion;

public class RadioQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(RadioQuestionHtmlModel question)
    {
        return View(question);
    }


    public class RadioQuestionHtmlModel : GenericQuestionViewComponent.GenericQuestionHtmlModel
    {
        private readonly Domain.Model.Questions.RadioQuestion _question;


        public static RadioQuestionHtmlModel FromDomainModel(Domain.Model.Questions.RadioQuestion question)
        {
            return new RadioQuestionHtmlModel(question);
        }

        private RadioQuestionHtmlModel(Domain.Model.Questions.RadioQuestion question) : base(question)
        {
            _question = question;
        }

        public override string? AnswerDisplayText => RadioAnswer;


        public IEnumerable<RadioOptionHtmlModel> RadioOptions => _question switch
        {
            Domain.Model.Questions.RadioQuestion q => q.Options.Select(o => new RadioOptionHtmlModel(
                o, 
                q.Answer == o,
                NestedQuestionHtmlModelFromDomain(o)
            )),
            _ => Enumerable.Empty<RadioOptionHtmlModel>(),
        };

        private static GenericQuestionViewComponent.GenericQuestionHtmlModel? NestedQuestionHtmlModelFromDomain(Domain.Model.Questions.RadioQuestion.RadioOption o)
        {
            return GenericQuestionViewComponent.GenericQuestionHtmlModel.FromDomain(o.NestedQuestion);
        }


        public string? RadioAnswer
        {
            get => _question.Answer?.Value;
            set => _question.Answer = _question.Options.FirstOrDefault(o => o.Value == value);
        }


        public class RadioOptionHtmlModel
        {
            private readonly Domain.Model.Questions.RadioQuestion.RadioOption _option;
            private readonly bool _isSelected;
            private readonly GenericQuestionViewComponent.GenericQuestionHtmlModel? _nestedQuestion;

            public RadioOptionHtmlModel(Domain.Model.Questions.RadioQuestion.RadioOption option, bool isSelected,
                GenericQuestionViewComponent.GenericQuestionHtmlModel? nestedQuestion)
            {
                _option = option;
                _isSelected = isSelected;
                _nestedQuestion = nestedQuestion;
            }

            public string DisplayTitle => _option.DisplayTitle;
            public string Value => _option.Value;
            public bool IsSelected => _isSelected;

            public GenericQuestionViewComponent.GenericQuestionHtmlModel? NestedQuestion => _nestedQuestion;
        }
    }
}
