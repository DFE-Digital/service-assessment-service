using Microsoft.AspNetCore.Mvc;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

public class GenericQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ServiceAssessmentService.WebApp.Pages.Book.EditModel.QuestionHtmlModel question)
    {
        return View(question);
    }
}
