using Microsoft.AspNetCore.Mvc;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.LongTextQuestion;

public class LongTextQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ServiceAssessmentService.WebApp.Pages.Book.EditModel.QuestionHtmlModel question)
    {
        return View(question);
    }
}
