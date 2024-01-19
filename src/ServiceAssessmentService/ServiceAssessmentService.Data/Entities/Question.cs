namespace ServiceAssessmentService.Data.Entities;

public class Question
{
    public Guid Id { get; set; }
    public Guid TemplatedFromId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string HintText { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Response { get; set; } = null;
}
