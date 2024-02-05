using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestDescriptionValidationTests
{
    [Fact]
    public void ValidateDescription_ReturnsValid_WhenDescriptionIsNotNull()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = "Test string",
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Empty(result.ValidationErrors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r")]
    [InlineData("\t\t")]
    [InlineData("\n\n")]
    [InlineData("\r\r")]
    [InlineData("\r\n")]
    [InlineData("\n\r")]
    public void ValidateDescription_ReturnsInvalid_WhenDescriptionIsNullOrEmptyOrWhitespace(string? description)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = description,
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.Description));
    }

    // tests to validate rejecting description values with ascii chars outside 32-126  range
    [Theory]
    [InlineData("Test description with \u0000 (null)")]
    [InlineData("Test description with \u0001 (start of heading)")]
    [InlineData("Test description with \u0002 (start of text)")]
    [InlineData("Test description with \u0003 (end of text)")]
    [InlineData("Test description with \u0004 (end of transmission)")]
    [InlineData("Test description with \u0005 (enquiry)")]
    [InlineData("Test description with \u0006 (acknowledge)")]
    [InlineData("Test description with \u0007 (bell)")]
    [InlineData("Test description with \u0008 (backspace)")]
    [InlineData("Test description with \u0009 (horizontal tab)")]
    // [InlineData("Test description with \u000A (line feed)")] // Commented out as newline characters are explicitly permitted
    [InlineData("Test description with \u000B (vertical tab)")]
    [InlineData("Test description with \u000C (form feed)")]
    // [InlineData("Test description with \u000D (carriage return)")] // Commented out as newline characters are explicitly permitted
    [InlineData("Test description with \u000E (shift out)")]
    [InlineData("Test description with \u000F (shift in)")]
    [InlineData("Test description with \u0010 (data link escape)")]
    [InlineData("Test description with \u0011 (device control 1)")]
    [InlineData("Test description with \u0012 (device control 2)")]
    [InlineData("Test description with \u0013 (device control 3)")]
    [InlineData("Test description with \u0014 (device control 4)")]
    [InlineData("Test description with \u0015 (negative acknowledge)")]
    [InlineData("Test description with \u0016 (synchronous idle)")]
    [InlineData("Test description with \u0017 (end of transmission block)")]
    [InlineData("Test description with \u0018 (cancel)")]
    [InlineData("Test description with \u0019 (end of medium)")]
    [InlineData("Test description with \u001A (substitute)")]
    [InlineData("Test description with \u001B (escape)")]
    [InlineData("Test description with \u001C (file separator)")]
    [InlineData("Test description with \u001D (group separator)")]
    [InlineData("Test description with \u001E (record separator)")]
    [InlineData("Test description with \u001F (unit separator)")]
    // Jump past 32-126 (hex 20-7E) range with printable punctuation and alphanumeric characters
    [InlineData("Test description with \u007F (delete)")]
    public void ValidateDescription_ReturnsWarning_WhenDescriptionContainsNonPrintableAsciiChars(string? description)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = description,
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationWarnings, v => v.FieldName == nameof(assessmentRequest.Description));
    }

    [Theory]
    [InlineData("Test description with \n (line feed)")]
    [InlineData("Test description with \r (carriage return)")]
    public void ValidateDescription_ReturnsValid_WhenDescriptionContainsNewlineCharacters(string? description)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = description,
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Empty(result.ValidationErrors);
    }


    [Fact]
    public void DescriptionMaximumLengthIs500Chars()
    {
        Assert.Equal(500, AssessmentRequest.DescriptionMaxLengthChars);
    }

    [Fact]
    public void ValidateDescription_ReturnsError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = new string('a', AssessmentRequest.DescriptionMaxLengthChars + 1),
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.Description));
    }

    [Fact]
    public void ValidateDescription_ReturnsValid_WhenDescriptionIsExactlyAtCharacterCountLimit()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = new string('a', AssessmentRequest.DescriptionMaxLengthChars),
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationErrors);
        Assert.Empty(result.ValidationWarnings);
    }

    [Fact]
    public void ValidateDescription_ReturnsValid_WhenDescriptionIsBelowCharacterCountLimit()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Description = new string('a', AssessmentRequest.DescriptionMaxLengthChars - 1),
        };

        // Act
        var result = assessmentRequest.ValidateDescription();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationErrors);
        Assert.Empty(result.ValidationWarnings);
    }
}
