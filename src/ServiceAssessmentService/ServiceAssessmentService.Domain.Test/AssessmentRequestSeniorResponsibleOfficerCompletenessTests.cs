using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestSeniorResponsibleOfficerCompletenessTests
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

    public static TheoryData<bool?, Person?, Person?> CompleteCombinations => new()
    {
        // Complete scenarios
        // - Declares DD as SRO, SRO details (correctly) absent and DD details provided
        {true, ArbitraryValidPerson, null}, // Declares DD is SRO, DD provided and nil SRO -> complete
        // - Declares DD is not the SRO, SRO details provided (DD details irrelevant)
        {false, null, ArbitraryValidPerson},
        {false, ArbitraryValidPerson, ArbitraryValidPerson},
        {false, ArbitraryIncompletePerson, ArbitraryValidPerson},

    };

    public static TheoryData<bool?, Person?, Person?> IncompleteCombinations => new()
    {
        // All other combinations are incomplete/invalid
        {null, null, null},
        {null, null, ArbitraryValidPerson},
        {null, ArbitraryValidPerson, null},
        {null, ArbitraryValidPerson, ArbitraryValidPerson},
        {null, ArbitraryIncompletePerson, null},
        {null, ArbitraryIncompletePerson, ArbitraryValidPerson},
        {null, ArbitraryValidPerson, ArbitraryIncompletePerson},
        {null, ArbitraryIncompletePerson, ArbitraryIncompletePerson},
        {true, null, null},
        {true, null, ArbitraryValidPerson},
        // {true, ArbitraryValidPerson, null}, // ignore - valid combination
        {true, ArbitraryValidPerson, ArbitraryValidPerson}, // Currently not valid to declare DD as SRO... AND also provide both details... May reconsider implementation later.
        {true, ArbitraryIncompletePerson, null},
        {true, ArbitraryIncompletePerson, ArbitraryValidPerson},
        {true, ArbitraryValidPerson, ArbitraryIncompletePerson},
        {true, ArbitraryIncompletePerson, ArbitraryIncompletePerson},
        {false, null, null},
        // {false, null, ArbitraryValidPerson}, // ignore - valid combination
        {false, ArbitraryValidPerson, null},
        // {false, ArbitraryValidPerson, ArbitraryValidPerson}, // ignore - valid combination
        {false, ArbitraryIncompletePerson, null},
        // {false, ArbitraryIncompletePerson, ArbitraryValidPerson}, // ignore - valid combination
        {false, ArbitraryValidPerson, ArbitraryIncompletePerson},
        {false, ArbitraryIncompletePerson, ArbitraryIncompletePerson},
    };


    [Theory]
    [MemberData(nameof(CompleteCombinations))]
    public void IsSeniorResponsibleOfficerComplete_WHEN_CompleteCombinationOfValues_THEN_IsComplete(
        bool? isDdTheSro,
        Person? deputyDirector,
        Person? seniorResponsibleOfficer
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = isDdTheSro,
            DeputyDirector = deputyDirector,
            SeniorResponsibleOfficer = seniorResponsibleOfficer,
        };

        // Act
        var result = assessmentRequest.IsSeniorResponsibleOfficerComplete();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncompleteCombinations))]
    public void IsSeniorResponsibleOfficerComplete_WHEN_IncompleteCombinationOfValues_THEN_IsComplete(
        bool? isDdTheSro,
        Person? deputyDirector,
        Person? seniorResponsibleOfficer
    )
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            IsDeputyDirectorTheSeniorResponsibleOfficer = isDdTheSro,
            DeputyDirector = deputyDirector,
            SeniorResponsibleOfficer = seniorResponsibleOfficer,
        };

        // Act
        var result = assessmentRequest.IsSeniorResponsibleOfficerComplete();

        // Assert
        string ddString = deputyDirector?.ToString() ?? "null";
        string sroString = seniorResponsibleOfficer?.ToString() ?? "null";

        Assert.False(result, $"Expected to be incomplete: " +
                             $"\n- Is DD the SRO:{isDdTheSro}" +
                             $"\n- DD: {ddString}" +
                             $"\n- SRO: {sroString}");
    }
}
