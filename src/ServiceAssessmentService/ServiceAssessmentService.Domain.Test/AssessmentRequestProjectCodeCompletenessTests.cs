using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestProjectCodeCompletenessTests
{
    private const string ArbitraryValidProjectCode = "Test string";

    // theory member data: bool?, string?, bool
    // is known, phase end date, expected is end date complete
    public static IEnumerable<object?[]> IsPhaseEndDateCompleteData =>
        new List<object?[]>
        {
            new object?[] {null, null, false},
            new object?[] {true, null, false},
            new object?[] {false, null, true},

            new object?[] {null, ArbitraryValidProjectCode, false},
            new object?[] {true, ArbitraryValidProjectCode, true},
            new object?[] {false, ArbitraryValidProjectCode, false},
        };

    [Theory]
    [MemberData(nameof(IsPhaseEndDateCompleteData))]
    public void IsPhaseEndDateComplete_ReturnsExpectedResult(
        bool? isProjectCodeKnown,
        string? projectCode,
        bool expectedResult
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = isProjectCodeKnown,
            ProjectCode = projectCode,
        };

        // Act
        var result = assessmentRequest.IsProjectCodeComplete();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
