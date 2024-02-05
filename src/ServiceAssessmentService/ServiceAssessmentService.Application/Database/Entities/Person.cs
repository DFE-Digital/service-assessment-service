namespace ServiceAssessmentService.Application.Database.Entities;

internal class Person
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
