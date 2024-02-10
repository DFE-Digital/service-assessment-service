using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestProductManagerCompletenessTests
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
        // - Declares Product Manager is known, Product Manager details provided
        {true, ArbitraryValidPerson},
        // - Declares Product Manager is not known, no Product Manager details provided
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
    public void IsProductManagerComplete_WHEN_CompleteCombinationOfValues_THEN_IsComplete(
        bool? hasProductManager,
        Person? productManager
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = hasProductManager,
            ProductOwnerManager = productManager,
        };

        // Act
        var result = assessmentRequest.IsProductOwnerManagerComplete();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncompleteCombinations))]
    public void IsProductManagerComplete_WHEN_IncompleteCombinationOfValues_THEN_IsComplete(
        bool? hasProductManager,
        Person? productManager
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            HasProductOwnerManager = hasProductManager,
            ProductOwnerManager = productManager,
        };

        // Act
        var result = assessmentRequest.IsProductOwnerManagerComplete();

        // Assert
        string productManagerString = productManager?.ToString() ?? "null";

        Assert.False(result, $"Expected to be incomplete: " +
                             $"\n- ProductManager: {productManagerString}");
    }
}
