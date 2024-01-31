namespace ServiceAssessmentService.Domain.Model;

public class ValidationError
{
    public string FieldName { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;
}
