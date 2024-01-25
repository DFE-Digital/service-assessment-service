using Microsoft.AspNetCore.Mvc;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.RadioQuestion;

public class RadioQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ServiceAssessmentService.WebApp.Pages.Book.EditModel.QuestionHtmlModel question)
    {
        return View(question);
    }
}
