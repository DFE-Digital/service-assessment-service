using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPersonalNameValidationTests
{
    [Fact]
    public void ValidateDeputyDirector_ReturnsValid_WhenAllPropertiesAreValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = "Alex",
                FamilyName = "Smith",
                Email = "a@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void NullDeputyDirector_IsValid_And_EmailErrorsReturned()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = null,
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.DeputyDirector)); }
        );
    }


    [Fact]
    public void ValidatePersonalName_ReturnsValid_And_NoPersonalNameErrorsOrWarnings_WhenPersonalNameIsNotNull()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = "Alex",
                FamilyName = null,
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.Multiple(
            // () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.PersonalNameWarnings); },
            () => { Assert.Empty(result.PersonalNameErrors); }
        );
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
    public void ValidatePersonalName_ReturnsInvalid_And_PersonalNameError_WhenPersonalNameIsNullOrEmptyOrWhitespace(string? personalName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = personalName,
                FamilyName = null,
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.PersonalNameErrors,
            v => v.FieldName == nameof(assessmentRequest.DeputyDirector.PersonalName));
    }

    // tests to validate rejecting personalName values with ascii chars outside 32-126  range
    [Theory]
    [InlineData("Test personalName with \u0000 (null)")]
    [InlineData("Test personalName with \u0001 (start of heading)")]
    [InlineData("Test personalName with \u0002 (start of text)")]
    [InlineData("Test personalName with \u0003 (end of text)")]
    [InlineData("Test personalName with \u0004 (end of transmission)")]
    [InlineData("Test personalName with \u0005 (enquiry)")]
    [InlineData("Test personalName with \u0006 (acknowledge)")]
    [InlineData("Test personalName with \u0007 (bell)")]
    [InlineData("Test personalName with \u0008 (backspace)")]
    [InlineData("Test personalName with \u0009 (horizontal tab)")]
    [InlineData("Test personalName with \u000A (line feed)")]
    [InlineData("Test personalName with \u000B (vertical tab)")]
    [InlineData("Test personalName with \u000C (form feed)")]
    [InlineData("Test personalName with \u000D (carriage return)")]
    [InlineData("Test personalName with \u000E (shift out)")]
    [InlineData("Test personalName with \u000F (shift in)")]
    [InlineData("Test personalName with \u0010 (data link escape)")]
    [InlineData("Test personalName with \u0011 (device control 1)")]
    [InlineData("Test personalName with \u0012 (device control 2)")]
    [InlineData("Test personalName with \u0013 (device control 3)")]
    [InlineData("Test personalName with \u0014 (device control 4)")]
    [InlineData("Test personalName with \u0015 (negative acknowledge)")]
    [InlineData("Test personalName with \u0016 (synchronous idle)")]
    [InlineData("Test personalName with \u0017 (end of transmission block)")]
    [InlineData("Test personalName with \u0018 (cancel)")]
    [InlineData("Test personalName with \u0019 (end of medium)")]
    [InlineData("Test personalName with \u001A (substitute)")]
    [InlineData("Test personalName with \u001B (escape)")]
    [InlineData("Test personalName with \u001C (file separator)")]
    [InlineData("Test personalName with \u001D (group separator)")]
    [InlineData("Test personalName with \u001E (record separator)")]
    [InlineData("Test personalName with \u001F (unit separator)")]
    // Jump past 32-126 (hex 20-7E) range with printable punctuation and alphanumeric characters
    [InlineData("Test personalName with \u007F (delete)")]
    public void ValidatePersonalName_IsValid_And_ReturnsWarning_WhenPersonalNameContainsNonPrintableAsciiChars(string? personalName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = personalName,
                FamilyName = null,
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.PersonalNameWarnings,
            v => v.FieldName == nameof(assessmentRequest.DeputyDirector.PersonalName));
    }


    [Fact]
    public void ValidateFamilyName_ReturnsValid_And_NoFamilyNameErrorsOrWarnings_WhenFamilyNameIsNotNull()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = "Alex",
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.Multiple(
            // () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.FamilyNameWarnings); },
            () => { Assert.Empty(result.FamilyNameErrors); }
        );
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
    public void ValidateFamilyName_ReturnsInvalid_AndFamilyNameError_WhenFamilyNameIsNullOrEmptyOrWhitespace(string? familyName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = familyName,
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.FamilyNameErrors,
            v => v.FieldName == nameof(assessmentRequest.DeputyDirector.FamilyName));
    }

    // tests to validate rejecting familyName values with ascii chars outside 32-126  range
    [Theory]
    [InlineData("Test familyName with \u0000 (null)")]
    [InlineData("Test familyName with \u0001 (start of heading)")]
    [InlineData("Test familyName with \u0002 (start of text)")]
    [InlineData("Test familyName with \u0003 (end of text)")]
    [InlineData("Test familyName with \u0004 (end of transmission)")]
    [InlineData("Test familyName with \u0005 (enquiry)")]
    [InlineData("Test familyName with \u0006 (acknowledge)")]
    [InlineData("Test familyName with \u0007 (bell)")]
    [InlineData("Test familyName with \u0008 (backspace)")]
    [InlineData("Test familyName with \u0009 (horizontal tab)")]
    [InlineData("Test familyName with \u000A (line feed)")]
    [InlineData("Test familyName with \u000B (vertical tab)")]
    [InlineData("Test familyName with \u000C (form feed)")]
    [InlineData("Test familyName with \u000D (carriage return)")]
    [InlineData("Test familyName with \u000E (shift out)")]
    [InlineData("Test familyName with \u000F (shift in)")]
    [InlineData("Test familyName with \u0010 (data link escape)")]
    [InlineData("Test familyName with \u0011 (device control 1)")]
    [InlineData("Test familyName with \u0012 (device control 2)")]
    [InlineData("Test familyName with \u0013 (device control 3)")]
    [InlineData("Test familyName with \u0014 (device control 4)")]
    [InlineData("Test familyName with \u0015 (negative acknowledge)")]
    [InlineData("Test familyName with \u0016 (synchronous idle)")]
    [InlineData("Test familyName with \u0017 (end of transmission block)")]
    [InlineData("Test familyName with \u0018 (cancel)")]
    [InlineData("Test familyName with \u0019 (end of medium)")]
    [InlineData("Test familyName with \u001A (substitute)")]
    [InlineData("Test familyName with \u001B (escape)")]
    [InlineData("Test familyName with \u001C (file separator)")]
    [InlineData("Test familyName with \u001D (group separator)")]
    [InlineData("Test familyName with \u001E (record separator)")]
    [InlineData("Test familyName with \u001F (unit separator)")]
    // Jump past 32-126 (hex 20-7E) range with printable punctuation and alphanumeric characters
    [InlineData("Test familyName with \u007F (delete)")]
    public void ValidateFamilyName_IsValid_And_ReturnsWarning_WhenFamilyNameContainsNonPrintableAsciiChars(string? familyName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = familyName,
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.FamilyNameWarnings, v => v.FieldName == nameof(assessmentRequest.DeputyDirector.FamilyName));
    }


    [Fact]
    public void ValidateEmail_IsValid_And_NoEmailErrorsOrWarnings_WhenEmailIsGoodValue()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = null,
                Email = "a@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.Multiple(
            // () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.EmailWarnings); },
            () => { Assert.Empty(result.EmailErrors); }
        );
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
    public void ValidateEmail_ReturnsInvalid_AndEmailError_WhenEmailIsNullOrEmptyOrWhitespace(string? email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = null,
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.EmailErrors, v => v.FieldName == nameof(assessmentRequest.DeputyDirector.Email));
    }

    // tests to validate rejecting email values with ascii chars outside 32-126  range
    [Theory]
    [InlineData("Test email with \u0000 (null)")]
    [InlineData("Test email with \u0001 (start of heading)")]
    [InlineData("Test email with \u0002 (start of text)")]
    [InlineData("Test email with \u0003 (end of text)")]
    [InlineData("Test email with \u0004 (end of transmission)")]
    [InlineData("Test email with \u0005 (enquiry)")]
    [InlineData("Test email with \u0006 (acknowledge)")]
    [InlineData("Test email with \u0007 (bell)")]
    [InlineData("Test email with \u0008 (backspace)")]
    [InlineData("Test email with \u0009 (horizontal tab)")]
    [InlineData("Test email with \u000A (line feed)")]
    [InlineData("Test email with \u000B (vertical tab)")]
    [InlineData("Test email with \u000C (form feed)")]
    [InlineData("Test email with \u000D (carriage return)")]
    [InlineData("Test email with \u000E (shift out)")]
    [InlineData("Test email with \u000F (shift in)")]
    [InlineData("Test email with \u0010 (data link escape)")]
    [InlineData("Test email with \u0011 (device control 1)")]
    [InlineData("Test email with \u0012 (device control 2)")]
    [InlineData("Test email with \u0013 (device control 3)")]
    [InlineData("Test email with \u0014 (device control 4)")]
    [InlineData("Test email with \u0015 (negative acknowledge)")]
    [InlineData("Test email with \u0016 (synchronous idle)")]
    [InlineData("Test email with \u0017 (end of transmission block)")]
    [InlineData("Test email with \u0018 (cancel)")]
    [InlineData("Test email with \u0019 (end of medium)")]
    [InlineData("Test email with \u001A (substitute)")]
    [InlineData("Test email with \u001B (escape)")]
    [InlineData("Test email with \u001C (file separator)")]
    [InlineData("Test email with \u001D (group separator)")]
    [InlineData("Test email with \u001E (record separator)")]
    [InlineData("Test email with \u001F (unit separator)")]
    // Jump past 32-126 (hex 20-7E) range with printable punctuation and alphanumeric characters
    [InlineData("Test email with \u007F (delete)")]
    public void ValidateEmail_IsValid_And_ReturnsWarning_WhenEmailContainsNonPrintableAsciiChars(string? email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = null,
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.EmailWarnings, v => v.FieldName == nameof(assessmentRequest.DeputyDirector.Email));
    }

    // Email format validation
    // - must contain @
    // - must have valid domain

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("a@")]
    public void ValidateEmail_IsInvalid_And_ReturnsError_WhenEmailDoesNotContainAtSymbolOrDomain(string email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = null,
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.EmailErrors, v => v.FieldName == nameof(assessmentRequest.DeputyDirector.Email));
    }

    [Theory]
    [InlineData("a@b")]
    [InlineData("a@b.c")]
    public void ValidateEmail_IsInvalid_And_ReturnsError_WhenEmailDomainIsNotOnValidList(string email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = null,
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.EmailErrors, v => v.FieldName == nameof(assessmentRequest.DeputyDirector.Email));
    }

    [Theory]
    [InlineData("a@education.gov.uk")]
    [InlineData("a@digital.education.gov.uk")]
    [InlineData("a@example.com")]
    [InlineData("a@example.org")]
    public void ValidateEmail_IsValid_And_NoEmailErrorsOrWarnings_WhenEmailDomainIsOnValidList(string email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            DeputyDirector = new Person()
            {
                PersonalName = null,
                FamilyName = null,
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeputyDirector();

        // Assert
        Assert.Multiple(
            // () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.EmailWarnings); },
            () => { Assert.Empty(result.EmailErrors); }
        );
    }

}
