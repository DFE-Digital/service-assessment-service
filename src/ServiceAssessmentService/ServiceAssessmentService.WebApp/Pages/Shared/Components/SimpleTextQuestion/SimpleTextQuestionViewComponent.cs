using Microsoft.AspNetCore.Mvc;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.SimpleTextQuestion;

public class SimpleTextQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ServiceAssessmentService.WebApp.Pages.Book.EditModel.QuestionHtmlModel question)
    {
        return View(question);
    }
}
