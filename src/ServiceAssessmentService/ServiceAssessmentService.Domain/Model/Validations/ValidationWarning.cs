namespace ServiceAssessmentService.Domain.Model.Validations;

public class ValidationWarning
{
    public string FieldName { get; set; } = null!;
    public string WarningMessage { get; set; } = null!;
}
