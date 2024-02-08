using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class PersonTest
{
    [Fact]
    public void FullName_WhenAllPropertiesAreValid_IsComplete()
    {
        // Arrange
        var person = new Person
        {
            PersonalName = "Alex",
            FamilyName = "Doe",
        };

        // Act
        var result = person.FullName();

        // Assert
        Assert.Equal("Alex Doe", result);
        Assert.True(person.IsNameComplete());
    }

    [Fact]
    public void FullName_WhenNoFamilyName_IsIncomplete()
    {
        // Arrange
        var person = new Person
        {
            PersonalName = "Alex",
            FamilyName = null,
        };

        // Act
        var result = person.FullName();

        // Assert
        Assert.Equal("Alex", result);
        Assert.False(person.IsNameComplete());
    }

    [Fact]
    public void FullName_WhenNoPersonalName_IsIncomplete()
    {
        // Arrange
        var person = new Person
        {
            PersonalName = null,
            FamilyName = "Doe",
        };

        // Act
        var result = person.FullName();

        // Assert
        Assert.Equal("Doe", result);
        Assert.False(person.IsNameComplete());
    }

    [Fact]
    public void FullName_WhenNoPersonalNameAndNoFamilyName_IsIncomplete()
    {
        // Arrange
        var person = new Person
        {
            PersonalName = null,
            FamilyName = null,
        };

        // Act
        var result = person.FullName();

        // Assert
        Assert.Null(result);
        Assert.False(person.IsNameComplete());
    }


}
