using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPhaseEndDateCompletenessTests
{
    private static DateOnly ArbirtraryValidDate => new DateOnly(2022, 1, 1);

    // theory member data: bool?, DateOnly?, bool
    // is known, phase end date, expected is end date complete
    public static IEnumerable<object?[]> IsPhaseEndDateCompleteData =>
        new List<object?[]>
        {
            new object?[] {null, null, false},
            new object?[] {true, null, false},
            new object?[] {false, null, true},

            new object?[] {null, ArbirtraryValidDate, false},
            new object?[] {true, ArbirtraryValidDate, true},
            new object?[] {false, ArbirtraryValidDate, false},
        };


    [Theory]
    [MemberData(nameof(IsPhaseEndDateCompleteData))]
    public void IsPhaseEndDateComplete_ReturnsExpectedResult(
        bool? isPhaseEndDateKnown,
        DateOnly? phaseEndDate,
        bool expectedResult
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsPhaseEndDateKnown = isPhaseEndDateKnown,
            PhaseEndDate = phaseEndDate,
        };

        // Act
        var result = assessmentRequest.IsPhaseEndDateComplete();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
