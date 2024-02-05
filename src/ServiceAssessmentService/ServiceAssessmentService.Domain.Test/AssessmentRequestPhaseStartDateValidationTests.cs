using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPhaseStartDateValidationTests
{
    private static DateOnly ArbitraryValidDate => new DateOnly(2022, 1, 1);

    [Fact]
    public void ArbitraryValidDateUsedInTestCases_IsBetweenPermittedDates()
    {
        // Arrange
        var earliestPermittedDate = AssessmentRequest.EarliestPermittedPhaseEndDate;
        var latestPermittedDate = AssessmentRequest.LatestPermittedPhaseEndDate;

        // Act
        var result = earliestPermittedDate <= ArbitraryValidDate && ArbitraryValidDate <= latestPermittedDate;

        // Assert
        Assert.True(result);
    }

    [Fact]

    public void IsPhaseStartDateValid_ReturnTrue_WhenPhaseStartDateIsNotNullOrEmptyOrWhitespace()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = ArbitraryValidDate,
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


    [Fact]
    public void When_PhaseStartDate_IsBefore_EarliestPermittedPhaseStartDate_Then_DateIsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = AssessmentRequest.EarliestPermittedPhaseStartDate.AddDays(-1),
        };

        // Act
        var result = assessmentRequest.ValidatePhaseStartDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);

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

    [Fact]
    public void When_PhaseStartDate_IsAfter_LatestPermittedPhaseStartDate_Then_DateIsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseStartDate = AssessmentRequest.LatestPermittedPhaseStartDate.AddDays(1),
        };

        // Act
        var result = assessmentRequest.ValidatePhaseStartDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);

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
