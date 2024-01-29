using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.Domain.Model.Questions;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.SimpleTextQuestion;

public class SimpleTextQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(SimpleTextQuestionHtmlModel question)
    {
        return View(question);
    }

    public class SimpleTextQuestionHtmlModel : GenericQuestionViewComponent.GenericQuestionHtmlModel
    {
        private readonly Domain.Model.Questions.SimpleTextQuestion _question;

        public static SimpleTextQuestionHtmlModel FromDomainModel(Domain.Model.Questions.SimpleTextQuestion question)
        {
            return new SimpleTextQuestionHtmlModel(question)
            {
                Id = question.Id,
            };
        }

        private SimpleTextQuestionHtmlModel(Domain.Model.Questions.SimpleTextQuestion question) : base(question)
        {
            _question = question;
        }

        public required Guid Id { get; set; }

        public override string? AnswerDisplayText => SimpleTextAnswer;

        public string? SimpleTextAnswer
        {
            get => _question.Answer;
            set => _question.Answer = value;
        }
    }
}
