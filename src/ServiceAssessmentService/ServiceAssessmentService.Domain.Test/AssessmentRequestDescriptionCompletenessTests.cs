using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestDescriptionCompletenessTests
{
    [Fact]
    public void IsDescriptionComplete_ReturnTrue_WhenDescriptionIsNotNullOrEmptyOrWhitespace()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = "Test description",
        };

        // Act
        var result = assessmentRequest.IsDescriptionComplete();

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
    public void IsDescriptionComplete_ReturnFalse_WhenDescriptionIsNullOrEmptyOrWhitespace(string? description)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = description,
        };

        // Act
        var result = assessmentRequest.IsDescriptionComplete();

        // Assert
        Assert.False(result);
    }
}
