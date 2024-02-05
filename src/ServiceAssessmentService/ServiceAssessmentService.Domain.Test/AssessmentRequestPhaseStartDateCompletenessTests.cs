using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPhaseStartDateCompletenessTests
{
    [Fact]
    public void IsPhaseStartDateComplete_ReturnTrue_WhenPhaseStartDateIsNotNullOrEmptyOrWhitespace()
    {
        // Arrange
        var arbitraryValidDate = new DateOnly(2022, 1, 1);
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = arbitraryValidDate,
        };

        // Act
        var result = assessmentRequest.IsPhaseStartDateComplete();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    public void IsPhaseStartDateComplete_ReturnFalse_WhenPhaseStartDateIsNotProvided(DateOnly? phaseStartDate)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = phaseStartDate,
        };

        // Act
        var result = assessmentRequest.IsPhaseStartDateComplete();

        // Assert
        Assert.False(result);
    }
}
