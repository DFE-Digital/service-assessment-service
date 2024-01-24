namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ProjectPhase? PhaseConcluding { get; set; }

    public AssessmentType? AssessmentType { get; set; }

    public DateOnly? PhaseStartDate { get; set; }

    public DateOnly? PhaseEndDate { get; set; }

    public string? Description { get; set; }


    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

}
