namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PhaseConcluding { get; set; } = string.Empty;

    public string AssessmentType { get; set; } = string.Empty;

    public DateOnly? PhaseStartDate { get; set; }

    public DateOnly? PhaseEndDate { get; set; }

    public string? Description { get; set; }


    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

}
