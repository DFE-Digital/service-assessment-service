using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.DateOnlyQuestion;

public class DateOnlyQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(DateOnlyQuestionHtmlModel question)
    {
        return View(question);
    }

    public class DateOnlyQuestionHtmlModel : GenericQuestionViewComponent.GenericQuestionHtmlModel
    {
        private readonly Domain.Model.Questions.DateOnlyQuestion _question;

        public static DateOnlyQuestionHtmlModel FromDomainModel(Domain.Model.Questions.DateOnlyQuestion question)
        {
            return new DateOnlyQuestionHtmlModel(question)
            {
                Id = question.Id,
            };
        }

        private DateOnlyQuestionHtmlModel(Domain.Model.Questions.DateOnlyQuestion question) : base(question)
        {
            _question = question;
        }

        public required Guid Id { get; set; }

        public override string? AnswerDisplayText => DateOnlyAnswer.ToString();

        public DateOnly? DateOnlyAnswer
        {
            get => _question.Answer;
            set => _question.Answer = value;
        }
    }
}
