using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestProjectCodeValidationTests
{
    private const string ArbitraryValidProjectCode = "Test string";

    [Fact]
    public void When_IsProjectCodeKnown_IsNull_And_ProjectCode_IsNull_Then_RadioIsNotValid_And_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = null,
            ProjectCode = null,
        };

        // Act
        var result = assessmentRequest.ValidateProjectCode();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.IsProjectCodeKnown));
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.ValidationErrors);
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }

    [Fact]
    public void When_IsProjectCodeKnown_IsFalse_And_ProjectCode_IsNull_Then_RadioIsValid_And_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = false,
            ProjectCode = null,
        };

        // Act
        var result = assessmentRequest.ValidateProjectCode();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.ValidationErrors);
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }

    [Fact]
    public void When_IsProjectCodeKnown_IsTrue_And_ProjectCode_IsNotNull_Then_RadioIsValid_And_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = true,
            ProjectCode = ArbitraryValidProjectCode,
        };

        // Act
        var result = assessmentRequest.ValidateProjectCode();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.ValidationErrors);
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }


    [Fact]
    public void When_IsProjectCodeKnown_IsTrue_And_ProjectCode_IsNull_Then_RadioIsValid_And_NestedIsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = true,
            ProjectCode = null,
        };

        // Act
        var result = assessmentRequest.ValidateProjectCode();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.False(result.NestedValidationResult.IsValid);
            Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.ProjectCode));
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }

    [Fact]
    public void When_IsProjectCodeKnown_IsFalse_And_ProjectCode_IsNotNull_Then_RadioIsValid_And_NestedIsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = false,
            ProjectCode = ArbitraryValidProjectCode,
        };

        // Act
        var result = assessmentRequest.ValidateProjectCode();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.RadioQuestionValidationErrors);
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.False(result.NestedValidationResult.IsValid);
            Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.ProjectCode));
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }

    [Fact]
    public void When_IsProjectCodeKnown_IsNull_And_ProjectCode_IsNotNull_Then_RadioIsNotValid_And_NestedIsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsProjectCodeKnown = null,
            ProjectCode = ArbitraryValidProjectCode,
        };

        // Act
        var result = assessmentRequest.ValidateProjectCode();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.IsProjectCodeKnown));
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.ValidationErrors);
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }
}
