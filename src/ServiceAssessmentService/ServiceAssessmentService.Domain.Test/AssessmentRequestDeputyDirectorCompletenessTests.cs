using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestDeputyDirectorCompletenessTests
{
    private const string? ArbitraryValidPersonalName = "Alex";
    private const string? ArbitraryValidFamilyName = "Doe";
    private const string? ArbitraryValidEmail = "a@example.com";

    [Fact]
    public void WhenDeputyDirectorIsNull_IsNotComplete()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest()
        {
            DeputyDirector = null,
        };

        // Act
        var result = assessmentRequest.IsDeputyDirectorComplete();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void WhenAllPersonPropertiesAreValid_IsComplete()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person
            {
                PersonalName = ArbitraryValidPersonalName,
                FamilyName = ArbitraryValidFamilyName,
                Email = ArbitraryValidEmail,
            },
        };

        // Act
        var result = assessmentRequest.IsDeputyDirectorComplete();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null, ArbitraryValidFamilyName, ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, null, ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, ArbitraryValidFamilyName, null)]
    [InlineData(null, null, ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, null, null)]
    [InlineData(null, ArbitraryValidFamilyName, null)]
    [InlineData(null, null, null)]
    [InlineData("", ArbitraryValidFamilyName, ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, "", ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, ArbitraryValidFamilyName, "")]
    [InlineData("", "", ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, "", "")]
    [InlineData("", ArbitraryValidFamilyName, "")]
    [InlineData("", "", "")]
    [InlineData(" ", ArbitraryValidFamilyName, ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, " ", ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, ArbitraryValidFamilyName, " ")]
    [InlineData(" ", " ", ArbitraryValidEmail)]
    [InlineData(ArbitraryValidPersonalName, " ", " ")]
    [InlineData(" ", ArbitraryValidFamilyName, " ")]
    [InlineData(" ", " ", " ")]
    public void WhenAnyPersonPropertyIsInvalid_IsNotComplete(string? personalName, string? familyName, string? email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person
            {
                PersonalName = personalName,
                FamilyName = familyName,
                Email = email,
            }
        };

        // Act
        var result = assessmentRequest.IsDeputyDirectorComplete();

        // Assert
        Assert.False(result);
    }
}
