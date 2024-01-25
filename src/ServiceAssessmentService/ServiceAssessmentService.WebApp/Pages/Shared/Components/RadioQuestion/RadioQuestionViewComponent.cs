using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Pages.Shared.Components.GenericQuestion;

namespace ServiceAssessmentService.WebApp.Pages.Shared.Components.RadioQuestion;

public class RadioQuestionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(GenericQuestionViewComponent.GenericQuestionHtmlModel question)
    {
        return View(question);
    }
}
