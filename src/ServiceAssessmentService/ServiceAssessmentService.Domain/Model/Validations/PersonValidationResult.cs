namespace ServiceAssessmentService.Domain.Model.Validations;

public class PersonValidationResult : ValidationResult
{
    public List<ValidationError> ValidationErrors { get; set; } = new();
    public List<ValidationWarning> ValidationWarnings { get; set; } = new();

    public List<ValidationError> PersonalNameErrors { get; set; } = new();
    public List<ValidationError> FamilyNameErrors { get; set; } = new();
    public List<ValidationError> EmailErrors { get; set; } = new();

    public List<ValidationWarning> PersonalNameWarnings { get; set; } = new();
    public List<ValidationWarning> FamilyNameWarnings { get; set; } = new();
    public List<ValidationWarning> EmailWarnings { get; set; } = new();
}
