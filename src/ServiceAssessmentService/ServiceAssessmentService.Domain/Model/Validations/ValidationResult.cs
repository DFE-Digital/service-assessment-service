namespace ServiceAssessmentService.Domain.Model.Validations;

public abstract class ValidationResult
{
    public virtual bool IsValid { get; set; } = true;
}
