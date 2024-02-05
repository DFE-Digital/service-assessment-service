namespace ServiceAssessmentService.Domain.Model.Validations;

public class RadioConditionalValidationResult<TNestedValidationResult> : ValidationResult where TNestedValidationResult : ValidationResult
{
    public List<ValidationWarning> RadioQuestionValidationWarnings { get; set; } = new();
    public List<ValidationError> RadioQuestionValidationErrors { get; set; } = new();

    public required TNestedValidationResult NestedValidationResult { get; set; }
}
