using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestAssessmentTypeValidationTests
{
    private readonly List<AssessmentType> _arbitraryTestAssessmentTypes = new()
    {
        new AssessmentType
        {
            Id = new Guid("00000000-1111-1111-1111-000000000000"),
            Name = "Test AssessmentType",
            DisplayNameMidSentenceCase = "test assessment type 1",
            SortOrder = 1,
        },
        new AssessmentType
        {
            Id = new Guid("00000000-2222-2222-2222-000000000000"),
            Name = "Test AssessmentType 2",
            DisplayNameMidSentenceCase = "test assessment type 2",
            SortOrder = 2,
        },
    };


    [Fact]
    public void When_AssessmentType_IsNull_Then_Result_IsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            AssessmentType = null,
        };

        // Act
        var result = assessmentRequest.ValidateAssessmentType(_arbitraryTestAssessmentTypes);

        // Assert
        Assert.False(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.AssessmentType));
    }

    [Fact]
    public void When_AssessmentType_IsNotNull_Then_Result_IsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            AssessmentType = _arbitraryTestAssessmentTypes.FirstOrDefault() ?? throw new Exception(),
        };

        // Act
        var result = assessmentRequest.ValidateAssessmentType(_arbitraryTestAssessmentTypes);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Empty(result.ValidationErrors);
    }

    [Fact]
    public void When_AssessmentType_IsNotInAssessmentTypes_Then_Result_IsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            AssessmentType = new AssessmentType
            {
                Id = new Guid("00000000-3333-3333-3333-000000000000"),
                Name = "Test AssessmentType 3",
                DisplayNameMidSentenceCase = "test assessment type 3",
                SortOrder = 3,
            },
        };

        // Act
        var result = assessmentRequest.ValidateAssessmentType(_arbitraryTestAssessmentTypes);

        // Assert
        Assert.False(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.AssessmentType));


    }
}
