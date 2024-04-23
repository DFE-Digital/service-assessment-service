namespace ServiceAssessmentService.Domain.Model;

public class PersonModel
{
    public Guid ID { get; set; }
    public string? PersonalName { get; set; }
    public string? FamilyName { get; set; }

    public string? FullName()
    {
        if (PersonalName is null && FamilyName is null)
        {
            return null;
        }

        if (PersonalName is null)
        {
            return FamilyName;
        }

        if (FamilyName is null)
        {
            return PersonalName;
        }

        return $"{PersonalName} {FamilyName}";
    }

    public string? Email { get; set; }

    public bool IsNameComplete()
    {
        return !string.IsNullOrWhiteSpace(PersonalName)
               && !string.IsNullOrWhiteSpace(FamilyName);
    }

    public bool IsComplete()
    {
        return IsNameComplete()
               && !string.IsNullOrWhiteSpace(Email);
    }
}
