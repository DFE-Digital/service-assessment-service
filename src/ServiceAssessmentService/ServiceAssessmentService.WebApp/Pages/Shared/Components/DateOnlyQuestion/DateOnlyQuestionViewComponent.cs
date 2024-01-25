using Microsoft.AspNetCore.Mvc;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.DateOnlyQuestion;

public class DateOnlyQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ServiceAssessmentService.WebApp.Pages.Book.EditModel.QuestionHtmlModel question)
    {
        return View(question);
    }
}
