using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

/// <summary>
/// - Must declare if there is a Product Manager
/// - When yes, must provide Product Manager details
/// - When no, Product Manager details must be absent
/// </summary>
public class AssessmentRequestProductOwnerManagerValidationTests
{

    private static PersonModel ArbitraryValidPerson => new PersonModel
    {
        PersonalName = "Arbitrary Personal Name",
        FamilyName = "Arbitrary Family Name",
        Email = "arbitrary@example.com",
    };




    [Fact]
    public void HappyPath_WHEN_HasProductOwnerManager_False_AND_ProductOwnerManager_Null_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = false,
            ProductOwnerManager = null,
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

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
    public void HappyPath_WHEN_HasProductOwnerManager_True_AND_ProductOwnerManager_Valid_THEN_RadioIsValid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = true,
            ProductOwnerManager = ArbitraryValidPerson,
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

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
    public void WHEN_HasProductOwnerManager_Null_AND_ProductOwnerManager_Null_THEN_RadioIsInvalid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = null,
            ProductOwnerManager = null,
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.HasProductOwnerManager)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.True(result.NestedValidationResult.IsValid); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationErrors); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    [Fact]
    public void WHEN_HasProductOwnerManager_True_AND_ProductOwnerManager_Null_THEN_RadioIsInvalid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = true,
            ProductOwnerManager = null,
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.HasProductOwnerManager)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.ProductOwnerManager)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // // has product manager true, product manager invalid personal name/family name/email, radio valid, nested invalid
    // [Theory]
    // [InlineData(true, null,null, null)]
    // [InlineData(true, null,null, "arbitrary@example.com")]
    // [InlineData(true, null,"Arbitrary Family Name", null)]
    // [InlineData(true, null,"Arbitrary Family Name", "arbitrary@example.com")]
    // [InlineData(true, "Arbitrary Personal Name",null, null)]
    // [InlineData(true, "Arbitrary Personal Name",null, "arbitrary@example.com")]
    // [InlineData(true, "Arbitrary Personal Name","Arbitrary Family Name", null)]
    // // [InlineData(true, "Arbitrary Personal Name","Arbitrary Family Name", "arbitrary@example.com")] // ignore - valid
    // public void WHEN_HasProductOwnerManager_True_AND_ProductOwnerManager_Invalid_THEN_RadioIsValid_AND_NestedIsInvalid(
    //     bool hasProductOwnerManager,
    //     string? personalName,
    //     string? familyName,
    //     string? email
    // )
    // {
    //     // Arrange
    //     var assessmentRequest = new AssessmentRequest
    //     {
    //         HasProductOwnerManager = hasProductOwnerManager,
    //         ProductOwnerManager = new Person
    //         {
    //             PersonalName = personalName,
    //             FamilyName = familyName,
    //             Email = email,
    //         },
    //     };
    //
    //     // Act
    //     var result = assessmentRequest.ValidateProductOwnerManager();
    //
    //     // Assert
    //     Assert.Multiple(
    //         () => { Assert.False(result.IsValid); },
    //         () => { Assert.Empty(result.RadioQuestionValidationErrors); },
    //         () => { Assert.Empty(result.RadioQuestionValidationWarnings); },
    //
    //         () => { Assert.False(result.NestedValidationResult.IsValid); },
    //         () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.ProductOwnerManager)); },
    //         () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
    //     );
    // }

    // has product manager true, product manager invalid personal name, radio valid, nested invalid
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void WHEN_HasProductOwnerManager_True_AND_ProductOwnerManager_InvalidPersonalName_THEN_RadioIsValid_AND_NestedIsInvalid(string? personalName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = true,
            ProductOwnerManager = new PersonModel
            {
                PersonalName = personalName,
                FamilyName = "Arbitrary Family Name",
                Email = "arbitrary@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.PersonalNameErrors, v => v.FieldName == nameof(assessmentRequest.ProductOwnerManager.PersonalName)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has product manager true, product manager invalid family name, radio valid, nested invalid
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void WHEN_HasProductOwnerManager_True_AND_ProductOwnerManager_InvalidFamilyName_THEN_RadioIsValid_AND_NestedIsInvalid(string? familyName)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = true,
            ProductOwnerManager = new PersonModel
            {
                PersonalName = "Arbitrary Personal Name",
                FamilyName = familyName,
                Email = "arbitrary@example.com",
            },
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.FamilyNameErrors, v => v.FieldName == nameof(assessmentRequest.ProductOwnerManager.FamilyName)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has product manager true, product manager invalid email, radio valid, nested invalid
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("invalid-email")]
    [InlineData("invalid-email@")]
    public void WHEN_HasProductOwnerManager_True_AND_ProductOwnerManager_InvalidEmail_THEN_RadioIsValid_AND_NestedIsInvalid(string? email)
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = true,
            ProductOwnerManager = new PersonModel
            {
                PersonalName = "Arbitrary Personal Name",
                FamilyName = "Arbitrary Family Name",
                Email = email,
            },
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

        // Assert
        Assert.Multiple(
            () => { Assert.True(result.IsValid); },
            () => { Assert.Empty(result.RadioQuestionValidationErrors); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.EmailErrors, v => v.FieldName == nameof(assessmentRequest.ProductOwnerManager.Email)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }

    // has product manager false, product manager valid, radio invalid, nested invalid
    [Fact]
    public void WHEN_HasProductOwnerManager_False_AND_ProductOwnerManager_Valid_THEN_RadioIsInvalid_AND_NestedIsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = false,
            ProductOwnerManager = ArbitraryValidPerson,
        };

        // Act
        var result = assessmentRequest.ValidateProductOwnerManager();

        // Assert
        Assert.Multiple(
            () => { Assert.False(result.IsValid); },
            () => { Assert.Contains(result.RadioQuestionValidationErrors, v => v.FieldName == nameof(assessmentRequest.HasProductOwnerManager)); },
            () => { Assert.Empty(result.RadioQuestionValidationWarnings); },

            () => { Assert.False(result.NestedValidationResult.IsValid); },
            () => { Assert.Contains(result.NestedValidationResult.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.ProductOwnerManager)); },
            () => { Assert.Empty(result.NestedValidationResult.ValidationWarnings); }
        );
    }




}
