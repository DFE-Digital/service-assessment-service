using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestNameValidationTests
{
    [Fact]
    public void ValidateName_ReturnsValid_WhenNameIsNotNull()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Name = "Test string",
        };

        // Act
        var result = assessmentRequest.ValidateName();

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
    public void ValidateName_ReturnsInvalid_WhenNameIsNullOrEmptyOrWhitespace(string? name)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Name = name,
        };

        // Act
        var result = assessmentRequest.ValidateName();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.Name));
    }

    // tests to validate rejecting name values with ascii chars outside 32-126  range
    [Theory]
    [InlineData("Test name with \u0000 (null)")]
    [InlineData("Test name with \u0001 (start of heading)")]
    [InlineData("Test name with \u0002 (start of text)")]
    [InlineData("Test name with \u0003 (end of text)")]
    [InlineData("Test name with \u0004 (end of transmission)")]
    [InlineData("Test name with \u0005 (enquiry)")]
    [InlineData("Test name with \u0006 (acknowledge)")]
    [InlineData("Test name with \u0007 (bell)")]
    [InlineData("Test name with \u0008 (backspace)")]
    [InlineData("Test name with \u0009 (horizontal tab)")]
    [InlineData("Test name with \u000A (line feed)")] // TODO: We may want to allow newline characters
    [InlineData("Test name with \u000B (vertical tab)")]
    [InlineData("Test name with \u000C (form feed)")]
    [InlineData("Test name with \u000D (carriage return)")] // TODO: We may want to allow newline characters
    [InlineData("Test name with \u000E (shift out)")]
    [InlineData("Test name with \u000F (shift in)")]
    [InlineData("Test name with \u0010 (data link escape)")]
    [InlineData("Test name with \u0011 (device control 1)")]
    [InlineData("Test name with \u0012 (device control 2)")]
    [InlineData("Test name with \u0013 (device control 3)")]
    [InlineData("Test name with \u0014 (device control 4)")]
    [InlineData("Test name with \u0015 (negative acknowledge)")]
    [InlineData("Test name with \u0016 (synchronous idle)")]
    [InlineData("Test name with \u0017 (end of transmission block)")]
    [InlineData("Test name with \u0018 (cancel)")]
    [InlineData("Test name with \u0019 (end of medium)")]
    [InlineData("Test name with \u001A (substitute)")]
    [InlineData("Test name with \u001B (escape)")]
    [InlineData("Test name with \u001C (file separator)")]
    [InlineData("Test name with \u001D (group separator)")]
    [InlineData("Test name with \u001E (record separator)")]
    [InlineData("Test name with \u001F (unit separator)")]
    // Jump past 32-126 (hex 20-7E) range with printable punctuation and alphanumeric characters
    [InlineData("Test name with \u007F (delete)")]
    public void ValidateName_ReturnsWarning_WhenNameContainsNonPrintableAsciiChars(string? name)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Name = name,
        };

        // Act
        var result = assessmentRequest.ValidateName();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationWarnings, v => v.FieldName == nameof(assessmentRequest.Name));
    }
}
