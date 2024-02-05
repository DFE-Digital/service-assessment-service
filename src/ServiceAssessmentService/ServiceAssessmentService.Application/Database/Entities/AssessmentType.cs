namespace ServiceAssessmentService.Application.Database.Entities;

internal class AssessmentType
{

    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string? DisplayNameMidSentenceCase { get; set; } = null;

    public int SortOrder { get; set; } = 0;

    public Domain.Model.AssessmentType? ToDomainModel()
    {
        return new Domain.Model.AssessmentType()
        {
            Id = Id,
            Name = Name,
            DisplayNameMidSentenceCase = DisplayNameMidSentenceCase,
            SortOrder = SortOrder,
        };
    }

    public static Database.Entities.AssessmentType? FromDomain(Domain.Model.AssessmentType? domainModel)
    {
        return new Database.Entities.AssessmentType()
        {
            Id = domainModel?.Id ?? Guid.Empty,
            Name = domainModel?.Name ?? string.Empty,
            DisplayNameMidSentenceCase = domainModel?.DisplayNameMidSentenceCase,
            SortOrder = domainModel?.SortOrder ?? 0,
        };
    }
}
