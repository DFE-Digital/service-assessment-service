namespace ServiceAssessmentService.Data.Entities;

public  class BaseEntity
{
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public DateTimeOffset? Deleted { get; set; }
}
