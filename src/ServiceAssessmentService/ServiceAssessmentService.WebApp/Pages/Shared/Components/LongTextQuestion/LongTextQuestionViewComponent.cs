using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.LongTextQuestion;

public class LongTextQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LongTextQuestionHtmlModel question)
    {
        return View(question);
    }

    public class LongTextQuestionHtmlModel : GenericQuestionViewComponent.GenericQuestionHtmlModel
    {
        private readonly Domain.Model.Questions.LongTextQuestion _question;

        public static LongTextQuestionHtmlModel FromDomainModel(Domain.Model.Questions.LongTextQuestion question)
        {
            return new LongTextQuestionHtmlModel(question);
        }

        private LongTextQuestionHtmlModel(Domain.Model.Questions.LongTextQuestion question) : base(question)
        {
            _question = question;
        }

        public override string? AnswerDisplayText => LongTextAnswer;

        public string? LongTextAnswer
        {
            get => _question.Answer;
            set => _question.Answer = value;
        }
    }
}
