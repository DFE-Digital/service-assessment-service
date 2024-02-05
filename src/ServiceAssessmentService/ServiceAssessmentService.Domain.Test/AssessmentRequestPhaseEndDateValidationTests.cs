using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

/// <summary>
/// - Must declare if phase end date is known
/// - When known, must provide a valid date
/// - When not known, end date must be null
/// </summary>
public class AssessmentRequestPhaseEndDateValidationTests
{
    private static DateOnly ArbitraryValidDate => new(2022, 1, 1);

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
    public void When_IsPhaseEndDateKnown_IsNull_RadioIsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = null,
            PhaseEndDate = null,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.IsPhaseEndDateKnown));
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.DateValidationErrors);
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void When_IsPhaseEndDateKnown_IsFalse_And_PhaseEndDate_IsNull_Then_RadioIsValid_And_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = false,
            PhaseEndDate = null,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.DateValidationErrors);
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void When_IsPhaseEndDateKnown_IsTrue_And_PhaseEndDate_IsNotNull_Then_RadioIsValid_And_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = true,
            PhaseEndDate = ArbitraryValidDate,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.DateValidationErrors);
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void When_IsPhaseEndDateKnown_IsTrue_And_PhaseEndDate_IsNull_Then_RadioIsValid_And_NestedIsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = true,
            PhaseEndDate = null,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            // Assume outer radio question is valid...
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            // ...and inner date question is invalid (missing)
            Assert.False(result.NestedValidationResult.IsValid);
            Assert.Contains(result.NestedValidationResult.DateValidationErrors,
                v => v.FieldName == nameof(assessmentRequest.PhaseEndDate));
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }


    [Fact]
    public void When_IsPhaseEndDateKnown_IsFalse_And_PhaseEndDate_IsNotNull_Then_RadioIsValid_And_NestedIsInvalid()
    {
        // Arrange
        var arbitraryValidDate = new DateOnly(2022, 1, 1);
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = false,
            PhaseEndDate = arbitraryValidDate,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            // Assume outer radio question is valid...
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            // ...and inner date question is invalid (should be null)
            Assert.False(result.NestedValidationResult.IsValid);
            Assert.Contains(result.NestedValidationResult.DateValidationErrors,
                v => v.FieldName == nameof(assessmentRequest.PhaseEndDate));
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void
        When_IsPhaseEndDateKnown_IsTrue_And_PhaseEndDate_IsEarliestPermittedDate_Then_RadioIsValid_And_NestedIsValid()
    {
        // Arrange
        var earliestPermittedDate = AssessmentRequest.EarliestPermittedPhaseEndDate;
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = true,
            PhaseEndDate = earliestPermittedDate,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.DateValidationErrors);
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void
        When_IsPhaseEndDateKnown_IsTrue_And_PhaseEndDate_IsBeforeEarliestPermittedDate_Then_RadioIsValid_And_NestedIsInvalid()
    {
        // Arrange
        var earliestPermittedDate = AssessmentRequest.EarliestPermittedPhaseEndDate;
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = true,
            PhaseEndDate = earliestPermittedDate.AddDays(-1),
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            // Assume outer radio question is valid...
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            // ...and inner date is invalid (should be valid date within permitted range)
            Assert.False(result.NestedValidationResult.IsValid);
            Assert.Contains(result.NestedValidationResult.DateValidationErrors, v => v.FieldName == nameof(assessmentRequest.PhaseEndDate));
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void
        When_IsPhaseEndDateKnown_IsTrue_And_PhaseEndDate_IsLatestPermittedDate_Then_RadioIsValid_And_NestedIsValid()
    {
        // Arrange
        var latestPermittedDate = AssessmentRequest.LatestPermittedPhaseEndDate;
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = true,
            PhaseEndDate = latestPermittedDate,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.DateValidationErrors);
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }

    [Fact]
    public void
        When_IsPhaseEndDateKnown_IsTrue_And_PhaseEndDate_IsAfterLatestPermittedDate_Then_RadioIsValid_And_NestedIsInvalid()
    {
        // Arrange
        var latestPermittedDate = AssessmentRequest.LatestPermittedPhaseEndDate;
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = true,
            PhaseEndDate = latestPermittedDate.AddDays(1),
        };

        // Act
        var result = assessmentRequest.ValidatePhaseEndDate();

        // Assert
        Assert.Multiple(() =>
        {
            // Assume outer radio question is valid...
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            // ...and inner date is invalid (should be valid date within permitted range)
            Assert.False(result.NestedValidationResult.IsValid);
            Assert.Contains(result.NestedValidationResult.DateValidationErrors, v => v.FieldName == nameof(assessmentRequest.PhaseEndDate));
            Assert.Empty(result.NestedValidationResult.DateValidationWarnings);
        });
    }
}
