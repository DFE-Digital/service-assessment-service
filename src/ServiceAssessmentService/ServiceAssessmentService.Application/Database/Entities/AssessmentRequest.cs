namespace ServiceAssessmentService.Application.Database.Entities;

internal class AssessmentRequest : BaseEntity
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;


    public Guid? PhaseConcludingId { get; set; } = null;
    public virtual Phase? PhaseConcluding { get; set; } = null!;

    public Guid? AssessmentTypeRequestedId { get; set; } = null;
    public virtual AssessmentType? AssessmentTypeRequested { get; set; } = null!;

    public bool? IsReassessment { get; set; } = null;

    public bool? IsProjectCodeKnown { get; set; } = null;
    public string? ProjectCode { get; set; } = null;

    public DateOnly? PhaseStartDate { get; set; }

    public bool? IsPhaseEndDateKnown { get; set; } = null;
    public DateOnly? PhaseEndDate { get; set; }

    public string? Description { get; set; }

    public List<DateOnly> RequestedReviewDates { get; set; } = new();

    public Guid? PortfolioId { get; set; } = null;
    public virtual Portfolio? Portfolio { get; set; } = null;

    public Guid? DeputyDirectorId { get; set; } = null;
    public virtual Person? DeputyDirector { get; set; } = null;

    public bool? IsDeputyDirectorTheSeniorResponsibleOfficer { get; set; } = null;
    public Guid? SeniorResponsibleOfficerId { get; set; } = null;
    public Person? SeniorResponsibleOfficer { get; set; } = null;


    public bool? HasProductOwnerManager { get; set; } = null;
    public Guid? ProductOwnerManagerId { get; set; } = null;
    public Person? ProductOwnerManager { get; set; } = null;

    public bool? HasDeliveryManager { get; set; } = null;
    public Guid? DeliveryManagerId { get; set; } = null;
    public Person? DeliveryManager { get; set; } = null;


    public ServiceAssessmentService.Domain.Model.AssessmentRequest ToDomainModel()
    {
        var domainModel = new ServiceAssessmentService.Domain.Model.AssessmentRequest
        {
            Id = Id,
            PhaseConcluding = PhaseConcluding?.ToDomainModel(),
            AssessmentType = AssessmentTypeRequested?.ToDomainModel(),
            Name = Name,
            Description = Description,
            IsProjectCodeKnown = IsProjectCodeKnown,
            ProjectCode = ProjectCode,
            PhaseStartDate = PhaseStartDate,
            IsPhaseEndDateKnown = IsPhaseEndDateKnown,
            PhaseEndDate = PhaseEndDate,
            Portfolio = Portfolio?.ToDomainModel(),
            DeputyDirector = DeputyDirector?.ToDomainModel(),
            CreatedAt = CreatedUtc,
            UpdatedAt = UpdatedUtc,
        };

        return domainModel;
    }

    public static Database.Entities.AssessmentRequest FromDomain(Domain.Model.AssessmentRequest domainModel)
    {
        var entity = new Database.Entities.AssessmentRequest
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Description = domainModel.Description,
            PhaseConcludingId = domainModel.PhaseConcluding?.Id,
            AssessmentTypeRequestedId = domainModel.AssessmentType?.Id,
            IsReassessment = domainModel.IsReassessment,
            IsProjectCodeKnown = domainModel.IsProjectCodeKnown,
            ProjectCode = domainModel.ProjectCode,
            PhaseStartDate = domainModel.PhaseStartDate,
            IsPhaseEndDateKnown = domainModel.IsPhaseEndDateKnown,
            PhaseEndDate = domainModel.PhaseEndDate,
            PortfolioId = domainModel.Portfolio?.Id,
            DeputyDirectorId = domainModel.DeputyDirector?.Id,
            CreatedUtc = domainModel.CreatedAt.UtcDateTime,
            UpdatedUtc = domainModel.UpdatedAt.UtcDateTime,
        };

        return entity;
    }
}
