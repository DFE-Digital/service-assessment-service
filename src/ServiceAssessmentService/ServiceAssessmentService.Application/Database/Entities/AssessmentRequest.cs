using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Application.Database.Entities;

internal class AssessmentRequest : BaseEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PhaseConcluding { get; set; } = string.Empty;

    public string AssessmentType { get; set; } = string.Empty;

    public DateOnly? PhaseStartDate { get; set; }

    public DateOnly? PhaseEndDate { get; set; }

    public string? Description { get; set; }


    public ServiceAssessmentService.Domain.Model.AssessmentRequest ToDomainModel()
    {
        var assessmentRequest = new ServiceAssessmentService.Domain.Model.AssessmentRequest
        {
            Id = Id,
            Name = Name,
            PhaseConcluding = ProjectPhase.FromName(PhaseConcluding),
            AssessmentType = ServiceAssessmentService.Domain.Model.AssessmentType.FromName(AssessmentType),
            PhaseStartDate = PhaseStartDate,
            PhaseEndDate = PhaseEndDate,
            Description = Description,
            CreatedAt = CreatedUtc,
            UpdatedAt = UpdatedUtc,
        };

        return assessmentRequest;
    }
}
