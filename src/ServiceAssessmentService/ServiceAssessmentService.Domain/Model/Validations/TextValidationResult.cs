namespace ServiceAssessmentService.Domain.Model.Validations;

public class TextValidationResult
{
    public bool IsValid { get; set; }

    public List<ValidationWarning> ValidationWarnings { get; set; } = new();
    public List<ValidationError> ValidationErrors { get; set; } = new();
}
