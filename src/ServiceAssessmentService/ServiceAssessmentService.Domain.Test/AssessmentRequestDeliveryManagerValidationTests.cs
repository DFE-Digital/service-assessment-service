using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

/// <summary>
/// - Must declare if there is a Delivery Manager
/// - When yes, must provide Delivery Manager details
/// - When no, Delivery Manager details must be absent
/// </summary>
public class AssessmentRequestDeliveryManagerValidationTests
{

    private static Person ArbitraryValidPerson => new Person
    {
        PersonalName = "Arbitrary Personal Name",
        FamilyName = "Arbitrary Family Name",
        Email = "arbitrary@example.com",
    };




    [Fact]
    public void HappyPath_WHEN_HasDeliveryManager_False_AND_DeliveryManager_Null_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = false,
            DeliveryManager = null,
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

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
    public void HappyPath_WHEN_HasDeliveryManager_True_AND_DeliveryManager_Valid_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = true,
            DeliveryManager = ArbitraryValidPerson,
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

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
    public void WHEN_HasDeliveryManager_Null_AND_DeliveryManager_Null_THEN_RadioIsInvalid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = null,
            DeliveryManager = null,
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.HasDeliveryManager)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.True(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    [Fact]
    public void WHEN_HasDeliveryManager_True_AND_DeliveryManager_Null_THEN_RadioIsInvalid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = true,
            DeliveryManager = null,
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.HasDeliveryManager)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.DeliveryManager)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has delivery manager true, delivery manager invalid personal name, radio valid, nested invalid
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void WHEN_HasDeliveryManager_True_AND_DeliveryManager_InvalidPersonalName_THEN_RadioIsValid_AND_NestedIsInvalid(string? personalName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = true,
            DeliveryManager = new Person
            {
                PersonalName = personalName,
                FamilyName = "Arbitrary Family Name",
                Email = "arbitrary@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.PersonalNameErrors, v => v.FieldName == nameof(assessmentRequest.DeliveryManager.PersonalName)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has delivery manager true, delivery manager invalid family name, radio valid, nested invalid
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void WHEN_HasDeliveryManager_True_AND_DeliveryManager_InvalidFamilyName_THEN_RadioIsValid_AND_NestedIsInvalid(string? familyName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = true,
            DeliveryManager = new Person
            {
                PersonalName = "Arbitrary Personal Name",
                FamilyName = familyName,
                Email = "arbitrary@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.FamilyNameErrors, v => v.FieldName == nameof(assessmentRequest.DeliveryManager.FamilyName)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has delivery manager true, delivery manager invalid email, radio valid, nested invalid
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("invalid-email")]
    [InlineData("invalid-email@")]
    public void WHEN_HasDeliveryManager_True_AND_DeliveryManager_InvalidEmail_THEN_RadioIsValid_AND_NestedIsInvalid(string? email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = true,
            DeliveryManager = new Person
            {
                PersonalName = "Arbitrary Personal Name",
                FamilyName = "Arbitrary Family Name",
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.EmailErrors, v => v.FieldName == nameof(assessmentRequest.DeliveryManager.Email)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has delivery manager false, delivery manager valid, radio invalid, nested invalid
    [Fact]
    public void WHEN_HasDeliveryManager_False_AND_DeliveryManager_Valid_THEN_RadioIsInvalid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasDeliveryManager = false,
            DeliveryManager = ArbitraryValidPerson,
        };

        // Act
        var result = assessmentRequest.ValidateDeliveryManager();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.HasDeliveryManager)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.DeliveryManager)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }




}
