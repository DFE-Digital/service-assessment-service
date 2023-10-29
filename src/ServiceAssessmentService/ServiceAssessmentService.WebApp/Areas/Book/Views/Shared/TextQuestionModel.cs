using ServiceAssessmentService.WebApp.Core;

namespace ServiceAssessmentService.WebApp.Areas.Book.Views.Shared;

public class TextQuestionModel
{
    public BookingRequestId? BookingRequestId { get; set; } = null;
    public string? InputName { get; set; }

    public string? QuestionText { get; set; }
    public string? QuestionHintText { get; set; }

    public string? FormController { get; set; }
    public string? FormAction { get; set; }

    public string? Value { get; set; } = null;

    public IEnumerable<string> Errors { get; set; } = new List<string>();
}
