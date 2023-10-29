namespace ServiceAssessmentService.WebApp.Services.Book;

public class ChangeRequestModel
{

    // TODO: Derive all this from an OpenAPI file, and include meta/body wrapper sections

    public bool IsSuccessful { get; set; }

    public List<Error> Errors { get; set; } = new List<Error>();
    public bool HasErrors => Errors.Any();


    // TODO: Ability to specify fields (e.g., an error specifically relating to the "month" component of a date, or to differentiate between the "project code" field where there is also a surrounding yes/no "is the project code known" field)
    public record Error(string Message);
}
