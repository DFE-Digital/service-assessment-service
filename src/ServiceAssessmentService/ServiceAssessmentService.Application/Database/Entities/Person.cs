
namespace ServiceAssessmentService.Application.Database.Entities;

public class Person
{
    public Guid Id { get; set; } = Guid.Empty;

    public string? PersonalName { get; set; } = string.Empty;

    public string? FamilyName { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public Domain.Model.PersonModel ToDomainModel()
    {
        return new Domain.Model.PersonModel
        {
            ID = Id,
            PersonalName = PersonalName,
            FamilyName = FamilyName,
            Email = Email
        };
    }

    public static Person FromDomainModel(Domain.Model.PersonModel person)
    {
        return new Person
        {
            Id = person.ID,
            PersonalName = person.PersonalName,
            FamilyName = person.FamilyName,
            Email = person.Email
        };
    }
}
