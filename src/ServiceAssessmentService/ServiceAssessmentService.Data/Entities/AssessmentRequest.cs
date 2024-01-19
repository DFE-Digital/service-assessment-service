namespace ServiceAssessmentService.Data.Entities;

public class AssessmentRequest
{
    public Guid Id { get; set; }


    public List<Question> Questions { get; set; } = new List<Question>();
}
