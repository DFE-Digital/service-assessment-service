
namespace ServiceAssessmentService.Application.Database.Entities;

internal class Person
{
    public Guid Id { get; set; } = Guid.Empty;

    public string? PersonalName { get; set; } = string.Empty;

    public string? FamilyName { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    internal Domain.Model.Person ToDomainModel()
    {
        return new Domain.Model.Person
        {
            Id = Id,
            PersonalName = PersonalName,
            FamilyName = FamilyName,
            Email = Email
        };
    }

    internal static Person FromDomainModel(Domain.Model.Person person)
    {
        return new Person
        {
            Id = person.Id,
            PersonalName = person.PersonalName,
            FamilyName = person.FamilyName,
            Email = person.Email
        };
    }
}
