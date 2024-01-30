namespace ServiceAssessmentService.Domain.Model;

public class AssessmentType
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string? DisplayNameMidSentenceCase { get; set; } = null;

    public int SortOrder { get; set; } = 0;
}
