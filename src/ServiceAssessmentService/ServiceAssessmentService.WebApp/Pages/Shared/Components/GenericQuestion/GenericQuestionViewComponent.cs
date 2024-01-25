using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.Domain.Model.Questions;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.DateOnlyQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.LongTextQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.RadioQuestion;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.SimpleTextQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

public class GenericQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(GenericQuestionHtmlModel question)
    {
        return View(question);
    }


    public abstract class GenericQuestionHtmlModel
    {
        private readonly Question _question;

        protected GenericQuestionHtmlModel(Question question)
        {
            _question = question;
        }

        public virtual string Title
        {
            get => _question.Title;
            set => _question.Title = value;
        }

        public virtual string HintText
        {
            get => _question.HintText;
            set => _question.HintText = value;
        }

        public virtual QuestionType Type
        {
            get => _question.Type;
            set => _question.Type = value;
        }
        
        public abstract string? AnswerDisplayText { get; }

        public static GenericQuestionHtmlModel? FromDomain(Question? question)
        {
            return question switch
            {
                Domain.Model.Questions.RadioQuestion q => RadioQuestionViewComponent.RadioQuestionHtmlModel.FromDomainModel(q),
                Domain.Model.Questions.DateOnlyQuestion q => DateOnlyQuestionViewComponent.DateOnlyQuestionHtmlModel.FromDomainModel(q),
                Domain.Model.Questions.LongTextQuestion q => LongTextQuestionViewComponent.LongTextQuestionHtmlModel.FromDomainModel(q),
                Domain.Model.Questions.SimpleTextQuestion q => SimpleTextQuestionViewComponent.SimpleTextQuestionHtmlModel.FromDomainModel(q),
                _ => null,
            };
        }
    }
}
