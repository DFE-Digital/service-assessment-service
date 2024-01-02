using AngleSharp.Dom;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Lookups;
using ServiceAssessmentService.WebApp.Test.TestHelpers;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test.Book.Request;

public class QuestionPhaseConcludingTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string Url = "/Book/BookingRequest/PhaseConcluding";

    private readonly WebApplicationFactory<Program> _factory;

    private static readonly IEnumerable<Phase> SamplePhases = new List<Phase>
    {
        new() { Id = "assessment_type_30", DisplayNameLowerCase = "Assessment Type 30", IsEnabled = false, SortOrder = 30},
        new() { Id = "assessment_type_10", DisplayNameLowerCase = "Assessment Type 10", IsEnabled = true, SortOrder = 10},
        new() { Id = "assessment_type_20", DisplayNameLowerCase = "Assessment Type 20", IsEnabled = false, SortOrder = 20},
    };


    public QuestionPhaseConcludingTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    private HttpClient ClientWithFakeApiReturningZeroPhases => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<ILookupsReadService, FakeLookupsReadServiceZeroPhases>();
            });
        })
        .CreateClient();

    private HttpClient ClientWithFakeApiReturningOnePhase => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<ILookupsReadService, FakeLookupsReadServiceOnePhase>();
            });
        })
        .CreateClient();

    private HttpClient ClientWithFakeApiReturningTwoPhases => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<ILookupsReadService, FakeLookupsReadServiceTwoPhases>();
            });
        })
        .CreateClient();

    private HttpClient ClientWithFakeApiReturningThreePhases => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<ILookupsReadService, FakeLookupsReadServiceThreePhases>();
            });
        })
        .CreateClient();


    [Fact]
    public async Task PageMustHaveSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(Url);

        // Assert
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue(); // Status Code 200-299

            // Redundant additional checks, but provides extra information in case of failure.
            int statusCode = (int)response.StatusCode;
            statusCode.Should().BeGreaterThanOrEqualTo(200);
            statusCode.Should().BeLessThanOrEqualTo(299);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }

    [Fact]
    public async Task QuestionTextHeadingMustBeAsExpected()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var heading = content.QuerySelector("h1");
        heading.Should().NotBeNull();
        heading!.TextContent.Trim().Should().Be("What phase are you currently in?");
    }

    [Fact]
    public async Task QuestionHintTextMustBeAsExpected()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var hintText = content.QuerySelector(".govuk-hint");
        hintText.Should().NotBeNull();
        var hintTextActualNormalised = TextHelper.NormaliseWhitespace(hintText!.TextContent);
        var hintTextExpectedNormalised = TextHelper.NormaliseWhitespace("Select one option. This is the phase you wish to be assessed.");

        hintTextActualNormalised.Should().Be(hintTextExpectedNormalised);
    }

    [Fact]
    public async Task GivenApiDefiningPhases_WhenApiGivesZeroPhases_THEN_MustBeZeroRadioElements()
    {
        // Arrange
        var client = ClientWithFakeApiReturningZeroPhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__label");
        availableRadioOptions.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact]
    public async Task GivenApiDefiningPhases_WhenApiGivesOnePhase_THEN_MustBeOneRadioElement()
    {
        // Arrange
        var client = ClientWithFakeApiReturningOnePhase;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__label");
        availableRadioOptions.Should().NotBeNull().And.HaveCount(1);
    }


    [Fact]
    public async Task GivenApiDefiningPhases_WhenApiGivesTwoPhases_THEN_MustBeTwoRadioElements()
    {
        // Arrange
        var client = ClientWithFakeApiReturningTwoPhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__label");
        availableRadioOptions.Should().NotBeNull().And.HaveCount(2);
    }


    [Fact]
    public async Task GivenApiDefiningPhases_WhenApiGivesThreePhases_THEN_MustBeThreeRadioElements()
    {
        // Arrange
        var client = ClientWithFakeApiReturningThreePhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__label");
        availableRadioOptions.Should().NotBeNull().And.HaveCount(3);
    }

    [Fact]
    public async Task GivenApiDefiningPhases_WhenOptionsAreDisplayed_THEN_MustHaveExpectedAnswerLabels()
    {
        // Arrange
        var client = ClientWithFakeApiReturningThreePhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__label");
        availableRadioOptions.Select(option => option.TextContent.Trim()).Should().BeEquivalentTo(
            "Assessment Type 10",
            "Assessment Type 20",
            "Assessment Type 30"
        );
    }

    [Fact]
    public async Task GivenApiDefiningPhases_WhenOptionsAreDisplayed_THEN_MustHaveExpectedAnswerLabelsInExpectedOrder()
    {
        // Arrange
        var client = ClientWithFakeApiReturningThreePhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__label");
        availableRadioOptions.Select(option => option.TextContent.Trim()).Should().Equal(
            "Assessment Type 10",
            "Assessment Type 20",
            "Assessment Type 30"
        );
    }

    [Fact]
    public async Task GivenApiDefiningPhases_WhenApiIndicatesOptionIsDisabled_THEN_InputMustBeEnabledDisabled()
    {
        // Arrange
        var client = ClientWithFakeApiReturningThreePhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__input");

        // NOTE: Previous implementation would disable the radios.
        // Instead, they now remain enabled but redirect to a "this type of assessment cannot be booked via this service" type page.
        availableRadioOptions.Select(option => option.IsDisabled()).Should().Equal(
            false,
            false,
            false
        );
    }

    [Fact]
    public async Task GivenApiDefiningPhases_WhenApiIndicatesSortOrder_THEN_InputValueMustMatchExpected()
    {
        // Arrange
        var client = ClientWithFakeApiReturningThreePhases;

        // Act
        var response = await client.GetAsync(Url);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        var availableRadioOptions = content.QuerySelectorAll(".govuk-radios__input");
        availableRadioOptions.Select(option => option.GetAttribute("value")).Should().Equal(
            "assessment_type_10",
            "assessment_type_20",
            "assessment_type_30"
        );
    }


    public class FakeLookupsReadServiceZeroPhases : FakeNotImplementedLookupsReadService
    {
        public override Task<IEnumerable<Phase>> GetPhases()
        {
            return Task.FromResult(Enumerable.Empty<Phase>());
        }
    }

    public class FakeLookupsReadServiceOnePhase : FakeNotImplementedLookupsReadService
    {
        public override Task<IEnumerable<Phase>> GetPhases()
        {
            return Task.FromResult(SamplePhases.Take(1));
        }
    }

    public class FakeLookupsReadServiceTwoPhases : FakeNotImplementedLookupsReadService
    {
        public override Task<IEnumerable<Phase>> GetPhases()
        {
            return Task.FromResult(SamplePhases.Take(2));
        }
    }

    public class FakeLookupsReadServiceThreePhases : FakeNotImplementedLookupsReadService
    {
        public override Task<IEnumerable<Phase>> GetPhases()
        {
            return Task.FromResult(SamplePhases);
        }
    }

}
