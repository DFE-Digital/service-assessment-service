using ServiceAssessmentService.WebApp.Core;

namespace ServiceAssessmentService.WebApp.Areas.Book.Views.Shared;

public class SimpleRadiosQuestionModel
{
    public BookingRequestId? BookingRequestId { get; set; }
    public string? InputName { get; set; }

    public string? QuestionText { get; set; }
    public string? QuestionHintText { get; set; }

    public string? FormController { get; set; }
    public string? FormAction { get; set; }

    public RadioItem? Value { get; set; }
    public IEnumerable<RadioItem> AllowedValues { get; set; } = new List<RadioItem>();

    public IEnumerable<string> Errors { get; set; } = new List<string>();

    public record RadioItem(string Id, string Value, string DisplayName, bool IsEnabled, int SortOrder);
}
