namespace ServiceAssessmentService.Domain.Model.Validations;

public class RadioValidationResult : ValidationResult
{
    public List<ValidationWarning> ValidationWarnings { get; set; } = new();
    public List<ValidationError> ValidationErrors { get; set; } = new();
}
