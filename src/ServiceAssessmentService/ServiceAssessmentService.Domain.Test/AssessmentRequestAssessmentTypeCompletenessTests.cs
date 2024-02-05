using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestAssessmentTypeCompletenessTests
{
    private static AssessmentType ArbitraryValidAssessmentType => new();

    [Fact]
    public void When_AssessmentType_IsNotNull_Then_IsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            AssessmentType = ArbitraryValidAssessmentType,
        };

        // Act
        var result = assessmentRequest.IsAssessmentTypeComplete();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void When_AssessmentType_IsNull_Then_IsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            AssessmentType = null,
        };

        // Act
        var result = assessmentRequest.IsAssessmentTypeComplete();

        // Assert
        Assert.False(result);

    }
}
