namespace ServiceAssessmentService.Application.Database.Entities;

internal class Phase
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string? DisplayNameMidSentenceCase { get; set; } = null;

    public int SortOrder { get; set; } = 0;

    public Domain.Model.Phase? ToDomainModel()
    {
        return new Domain.Model.Phase()
        {
            Id = Id,
            Name = Name,
            DisplayNameMidSentenceCase = DisplayNameMidSentenceCase,
            SortOrder = SortOrder,
        };
    }

    public static Database.Entities.Phase? FromDomain(Domain.Model.Phase? domainModel)
    {
        if (domainModel == null)
        {
            return null;
        }

        return new Database.Entities.Phase()
        {
            Id = domainModel?.Id ?? Guid.Empty,
            Name = domainModel?.Name ?? string.Empty,
            DisplayNameMidSentenceCase = domainModel?.DisplayNameMidSentenceCase,
            SortOrder = domainModel?.SortOrder ?? 0,
        };
    }
}
