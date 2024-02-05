using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPhaseConcludingCompletenessTests
{
    private static Phase ArbitraryValidPhaseConcluding => new();

    [Fact]
    public void When_PhaseConcluding_IsNotNull_Then_IsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseConcluding = ArbitraryValidPhaseConcluding,
        };

        // Act
        var result = assessmentRequest.IsPhaseConcludingComplete();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void When_PhaseConcluding_IsNull_Then_IsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseConcluding = null,
        };

        // Act
        var result = assessmentRequest.IsPhaseConcludingComplete();

        // Assert
        Assert.False(result);

    }
}
