namespace ServiceAssessmentService.Domain.Model.Validations;


public class DateValidationResult
{
    public bool IsValid { get; set; } = true;

    public List<ValidationError> YearValidationErrors { get; set; } = new();
    public List<ValidationError> MonthValidationErrors { get; set; } = new();
    public List<ValidationError> DayValidationErrors { get; set; } = new();
    public List<ValidationError> DateValidationErrors { get; set; } = new();

    public List<ValidationWarning> YearValidationWarnings { get; set; } = new();
    public List<ValidationWarning> MonthValidationWarnings { get; set; } = new();
    public List<ValidationWarning> DayValidationWarnings { get; set; } = new();
    public List<ValidationWarning> DateValidationWarnings { get; set; } = new();
}
