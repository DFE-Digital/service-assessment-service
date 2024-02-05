using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPhaseStartDateValidationTests
{
    [Fact]

    public void IsPhaseStartDateValid_ReturnTrue_WhenPhaseStartDateIsNotNullOrEmptyOrWhitespace()
    {
        // Arrange
        var arbitraryValidDate = new DateOnly(2022, 1, 1);
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = arbitraryValidDate,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseStartDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);

            Assert.Empty(result.DateValidationErrors);
            Assert.Empty(result.DayValidationErrors);
            Assert.Empty(result.MonthValidationErrors);
            Assert.Empty(result.YearValidationErrors);

            Assert.Empty(result.DateValidationWarnings);
            Assert.Empty(result.DayValidationWarnings);
            Assert.Empty(result.MonthValidationWarnings);
            Assert.Empty(result.YearValidationWarnings);
        });
    }

    [Theory]
    [InlineData(null)]
    public void IsPhaseStartDateValid_ReturnFalse_WhenPhaseStartDateIsNullOrEmptyOrWhitespace(DateOnly? phaseStartDate)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = phaseStartDate,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseStartDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);

            // Whole date not provided - therefore error on whole date only, not any individual part
            Assert.Contains(result.DateValidationErrors, v => v.FieldName == nameof(assessmentRequest.PhaseStartDate));
            Assert.Empty(result.DayValidationErrors);
            Assert.Empty(result.MonthValidationErrors);
            Assert.Empty(result.YearValidationErrors);

            Assert.Empty(result.DateValidationWarnings);
            Assert.Empty(result.DayValidationWarnings);
            Assert.Empty(result.MonthValidationWarnings);
            Assert.Empty(result.YearValidationWarnings);
        });
    }
}
