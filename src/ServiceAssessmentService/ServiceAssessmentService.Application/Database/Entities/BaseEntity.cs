namespace ServiceAssessmentService.Application.Database.Entities;

internal class BaseEntity
{
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public DateTimeOffset? Deleted { get; set; }
}
