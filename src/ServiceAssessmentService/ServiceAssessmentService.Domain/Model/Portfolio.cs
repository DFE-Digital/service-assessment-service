namespace ServiceAssessmentService.Domain.Model;

public class Portfolio
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;
    
    public string DisplayNameMidSentenceCase { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;
    
    public bool IsInternalGroup { get; set; } = false;
}
