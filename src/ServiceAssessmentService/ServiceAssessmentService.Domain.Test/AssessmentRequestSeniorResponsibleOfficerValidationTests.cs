using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

/// <summary>
/// - Must declare if DD is the Senior Responsible Officer
/// - When no, must provide SRO details
/// - When yes, DD details must be provided and SRO details must be absent
/// </summary>
public class AssessmentRequestSeniorResponsibleOfficerValidationTests
{

    private static Person ArbitraryValidPerson => new Person
    {
        PersonalName = "Arbitrary Personal Name",
        FamilyName = "Arbitrary Family Name",
        Email = "arbitrary@example.com",
    };


    [Fact]
    public void WHEN_DdIsSro_True_AND_Sro_Null_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = true,
            DeputyDirector = ArbitraryValidPerson,
            SeniorResponsibleOfficer = null,
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.True(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }


    [Fact]
    public void WHEN_DdIsSro_True_AND_Sro_MissingPersonalName_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = null,
            // DeputyDirector = null, // should not be relevant
            SeniorResponsibleOfficer = null,
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.IsDeputyDirectorTheSeniorResponsibleOfficer));
            Assert.Empty(result.RadioQuestionValidationWarnings);

            Assert.True(result.NestedValidationResult.IsValid);
            Assert.Empty(result.NestedValidationResult.ValidationErrors);
            Assert.Empty(result.NestedValidationResult.ValidationWarnings);
        });
    }

    [Fact]
    public void WHEN_DdIsSro_True_AND_Dd_Null_AND_Sro_Null_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = true,
            DeputyDirector = null,
            SeniorResponsibleOfficer = null,
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.IsDeputyDirectorTheSeniorResponsibleOfficer)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.True(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    [Fact]
    public void WHEN_DdIsSro_True_AND_Dd_Valid_AND_Sro_Valid_THEN_RadioIsInvalid_AND_NestedIsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = true,
            DeputyDirector = ArbitraryValidPerson,
            SeniorResponsibleOfficer = ArbitraryValidPerson,
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.IsDeputyDirectorTheSeniorResponsibleOfficer)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.SeniorResponsibleOfficer)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    [Fact]
    public void WHEN_DdIsSro_False_AND_Sro_Null_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = false,
            // DeputyDirector = null, // should not be relevant
            SeniorResponsibleOfficer = null,
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.SeniorResponsibleOfficer)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    [Fact]
    public void WHEN_DdIsSro_False_AND_Sro_MissingPersonalName_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = false,
            // DeputyDirector = null, // should not be relevant
            SeniorResponsibleOfficer = new()
            {
                PersonalName = null,
                FamilyName = "Arbitrary Family Name",
                Email = "arbitrary@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); },
            () => { Assert.Contains(result.NestedValidationResult.PersonalNameErrors, v => v.FieldName == nameof(assessmentRequest.SeniorResponsibleOfficer.PersonalName)); },
            () => { Assert.Empty(result.NestedValidationResult.PersonalNameWarnings); }
        );
    }

    [Fact]
    public void WHEN_DdIsSro_False_AND_Sro_MissingFamilyName_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = false,
            // DeputyDirector = null, // should not be relevant
            SeniorResponsibleOfficer = new()
            {
                PersonalName = "Arbitrary Personal Name",
                FamilyName = null,
                Email = "arbitrary@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); },
            () => { Assert.Contains(result.NestedValidationResult.FamilyNameErrors, v => v.FieldName == nameof(assessmentRequest.SeniorResponsibleOfficer.FamilyName)); },
            () => { Assert.Empty(result.NestedValidationResult.FamilyNameWarnings); }
        );
    }

    [Fact]
    public void WHEN_DdIsSro_False_AND_Sro_MissingEmail_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = false,
            // DeputyDirector = null, // should not be relevant
            SeniorResponsibleOfficer = new()
            {
                PersonalName = "Arbitrary Personal Name",
                FamilyName = "Arbitrary Family Name",
                Email = null,
            },
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); },
            () => { Assert.Contains(result.NestedValidationResult.EmailErrors, v => v.FieldName == nameof(assessmentRequest.SeniorResponsibleOfficer.Email)); },
            () => { Assert.Empty(result.NestedValidationResult.EmailWarnings); }
        );
    }

    [Fact]
    public void WHEN_DdIsSro_False_AND_Sro_Valid_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = false,
            // DeputyDirector = null, // should not be relevant
            SeniorResponsibleOfficer = ArbitraryValidPerson,
        };

        // Act
        var result = assessmentRequest.ValidateSeniorResponsibleOfficer();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.True(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

}
