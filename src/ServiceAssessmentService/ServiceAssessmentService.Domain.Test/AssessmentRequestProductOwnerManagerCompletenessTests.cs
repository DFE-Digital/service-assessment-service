using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestProductOwnerManagerCompletenessTests
{
    private static Person ArbitraryValidPerson => new Person
    {
        PersonalName = "Arbitrary Personal Name",
        FamilyName = "Arbitrary Family Name",
        Email = "arbitrary@example.com",
    };
    private static Person ArbitraryIncompletePerson => new Person
    {
        PersonalName = null,
        FamilyName = null,
        Email = null,
    };

    public static TheoryData<bool?, Person?> CompleteCombinations => new()
    {
        // Complete scenarios
        // - Declares Product Owner/Manager is known, Product Owner/Manager details provided
        {true, ArbitraryValidPerson},
        // - Declares Product Owner/Manager is not known, no Product Owner/Manager details provided
        {false, null},

    };

    public static TheoryData<bool?, Person?> IncompleteCombinations => new()
    {
        // All other combinations are incomplete/invalid
        {null, null},
        {null, ArbitraryValidPerson},
        {null, ArbitraryIncompletePerson},

        {true, null},
        // {true, ArbitraryValidPerson}, // ignore - valid combination
        {true, ArbitraryIncompletePerson},
        
        // {false, null}, // ignore - valid combination
        {false, ArbitraryValidPerson},
        {false, ArbitraryIncompletePerson},

    };


    [Theory]
    [MemberData(nameof(CompleteCombinations))]
    public void IsProductOwnerManagerComplete_WHEN_CompleteCombinationOfValues_THEN_IsComplete(
        bool? hasProductOwnerManager,
        Person? productOwnerManager
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = hasProductOwnerManager,
            ProductOwnerManager = productOwnerManager,
        };

        // Act
        var result = assessmentRequest.IsProductOwnerManagerComplete();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncompleteCombinations))]
    public void IsProductOwnerManagerComplete_WHEN_IncompleteCombinationOfValues_THEN_IsComplete(
        bool? hasProductOwnerManager,
        Person? productOwnerManager
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = hasProductOwnerManager,
            ProductOwnerManager = productOwnerManager,
        };

        // Act
        var result = assessmentRequest.IsProductOwnerManagerComplete();

        // Assert
        string productOwnerManagerString = productOwnerManager?.ToString() ?? "null";

        Assert.False(result, $"Expected to be incomplete: " +
                             $"\n- ProductOwnerManager: {productOwnerManagerString}");
    }
}
