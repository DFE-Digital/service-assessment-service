using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestNameCompletenessTests
{
    [Fact]
    public void IsNameComplete_ReturnTrue_WhenNameIsNotNullOrEmptyOrWhitespace()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Name = "Test name",
        };

        // Act
        var result = assessmentRequest.IsNameComplete();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r")]
    [InlineData("\t\t")]
    [InlineData("\n\n")]
    [InlineData("\r\r")]
    [InlineData("\r\n")]
    [InlineData("\n\r")]
    public void IsNameComplete_ReturnFalse_WhenNameIsNullOrEmptyOrWhitespace(string? name)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Name = name,
        };

        // Act
        var result = assessmentRequest.IsNameComplete();

        // Assert
        Assert.False(result);
    }
}
