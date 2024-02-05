using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPhaseConcludingValidationTests
{
    private readonly List<Phase> _arbitraryTestPhases = new()
    {
        new Phase
        {
            Id = new Guid("00000000-1111-1111-1111-000000000000"),
            Name = "Test Phase",
            DisplayNameMidSentenceCase = "test phase 1",
            SortOrder = 1,
        },
        new Phase
        {
            Id = new Guid("00000000-2222-2222-2222-000000000000"),
            Name = "Test Phase 2",
            DisplayNameMidSentenceCase = "test phase 2",
            SortOrder = 2,
        },
    };


    [Fact]
    public void When_PhaseConcluding_IsNull_Then_Result_IsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseConcluding = null,
        };

        // Act
        var result = assessmentRequest.ValidatePhaseConcluding(_arbitraryTestPhases);

        // Assert
        Assert.False(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.PhaseConcluding));
    }

    [Fact]
    public void When_PhaseConcluding_IsNotNull_Then_Result_IsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseConcluding = _arbitraryTestPhases.FirstOrDefault() ?? throw new Exception(),
        };

        // Act
        var result = assessmentRequest.ValidatePhaseConcluding(_arbitraryTestPhases);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Empty(result.ValidationErrors);
    }

    [Fact]
    public void When_PhaseConcluding_IsNotInPhases_Then_Result_IsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            PhaseConcluding = new Phase
            {
                Id = new Guid("00000000-3333-3333-3333-000000000000"),
                Name = "Test Phase 3",
                DisplayNameMidSentenceCase = "test phase 3",
                SortOrder = 3,
            },
        };

        // Act
        var result = assessmentRequest.ValidatePhaseConcluding(_arbitraryTestPhases);

        // Assert
        Assert.False(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.PhaseConcluding));


    }
}
