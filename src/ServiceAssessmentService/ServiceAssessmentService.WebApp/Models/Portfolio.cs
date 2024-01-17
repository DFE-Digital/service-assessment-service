namespace ServiceAssessmentService.WebApp.Models;

public class Portfolio
{
    public string Id { get; init; } = null!;
    public string DisplayName { get; init; } = null!;
    public bool IsEnabled { get; init; } = false;
    public int SortOrder { get; init; } = 0;
    public bool IsInMainSection { get; init; } = false;
}
