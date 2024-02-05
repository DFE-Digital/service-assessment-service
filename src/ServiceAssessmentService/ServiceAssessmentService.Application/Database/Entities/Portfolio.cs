namespace ServiceAssessmentService.Application.Database.Entities;

internal class Portfolio
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string DisplayNameMidSentenceCase { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;

    public bool IsInternalGroup { get; set; } = false;


    public static Database.Entities.Portfolio FromDomain(Domain.Model.Portfolio domainModel)
    {
        return new Database.Entities.Portfolio()
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            DisplayNameMidSentenceCase = domainModel.DisplayNameMidSentenceCase,
            SortOrder = domainModel.SortOrder,
            IsInternalGroup = domainModel.IsInternalGroup,
        };
    }

    public Domain.Model.Portfolio ToDomainModel()
    {
        return new Domain.Model.Portfolio()
        {
            Id = Id,
            Name = Name,
            DisplayNameMidSentenceCase = DisplayNameMidSentenceCase,
            SortOrder = SortOrder,
            IsInternalGroup = IsInternalGroup,
        };
    }
}
