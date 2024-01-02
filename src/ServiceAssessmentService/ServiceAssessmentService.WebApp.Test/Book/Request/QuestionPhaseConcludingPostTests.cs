using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ServiceAssessmentService.WebApp.Controllers.Book;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Lookups;
using ServiceAssessmentService.WebApp.Test.TestHelpers;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test.Book.Request;

public class QuestionPhaseConcludingPostTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string Url = "/Book/BookingRequest/PhaseConcluding";

    private readonly WebApplicationFactory<Program> _factory;

    private static readonly IEnumerable<Phase> SamplePhases = new List<Phase>
    {
        new() { Id = "phase_30", DisplayNameLowerCase = "Phase 30", IsEnabled = false, SortOrder = 30},
        new() { Id = "phase_10", DisplayNameLowerCase = "Phase 10", IsEnabled = true, SortOrder = 10},
        new() { Id = "phase_20", DisplayNameLowerCase = "Phase 20", IsEnabled = false, SortOrder = 20},
    };

    public QuestionPhaseConcludingPostTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    private WebApplicationFactory<Program> FactoryWithFakeApiReturningThreePhases => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<ILookupsReadService, FakeLookupsReadServiceThreePhases>();
            });
        });



    [Fact]
    public async Task GivenApiDefiningPhases_WhenSubmittingValidValues_ResponseCodeMustBeRedirect()
    {
        // Arrange
        var client = FactoryWithFakeApiReturningThreePhases
            .CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    // On form submission, should receive POST and redirect -- do not follow this redirect
                    AllowAutoRedirect = false,
                }
            );

        // Must perform post after a get, to first generate/fetch/include the CSRF token (used within the subsequent post)
        var responseGet = await client.GetAsync(Url);
        var contentGet = await HtmlHelpers.GetDocumentAsync(responseGet);

        // Get the (single) form/submit button from within the page body (ignoring, e.g., forms within the header/footer).
        // If more than one is found, this is a problem and should be flagged as an error.
        var mainPageContentSection = contentGet.QuerySelectorAll(".main--content").Single();
        var formElement = mainPageContentSection.QuerySelectorAll<IHtmlFormElement>("form").Single();
        var submitButtonElement = formElement.QuerySelectorAll<IHtmlButtonElement>("button[type='submit']").Single();

        // Set one of the values to "checked" (i.e., radio button is selected)
        var arbitraryIdFromApi = SamplePhases.Where(x => x.IsEnabled).Take(1).Single().Id;
        var radioElements = formElement.QuerySelectorAll<IHtmlInputElement>("input[name='" + PhaseConcludingDto.FormName + "']");
        var peerReviewRadioElement = radioElements.Single(x => arbitraryIdFromApi.Equals(x.GetAttribute("value"), StringComparison.Ordinal));
        peerReviewRadioElement.IsChecked = true;


        // Act
        var postResponse = await client.SendAsync(formElement, submitButtonElement);

        // Assert
        // TODO: Consider extracting this out to helper method, to avoid duplication across tests?
        using (new AssertionScope())
        {
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect, "because a successful form submission should accept/process the POST then redirect to the next page (preventing the user from refreshing the browser and re-submitting data)");

            // Redundant additional checks, but provides extra information in case of failure.
            int statusCode = (int)postResponse.StatusCode;
            statusCode.Should().NotBe(400, "because if this is returned, it indicates the form value was not received (likely developer/test error, a mistake in setting up the fake API responses perhaps? note that if the radio/option is disabled, the value does not get submitted with the form)");
            statusCode.Should().NotBe(422, "because if this is returned, an unexpected/unprocessable form value was submitted (likely developer/test error)");
        }
    }

    // TODO: Validate where the form redirects to on a successful submit
    // - should be the next page in the flow, e.g., the "request started" question page or the "select project phase" question page (if implemented)

    // TODO: Consider side-effects (trigger of a write API request)
    // - Current flow is that a successful submit creates a new assessment request
    // - Next implementations will ask about the phase (which may/may not defer creating a new assessment request)

    // TODO: Error cases
    // - No form value
    // - Wholly inappropriate value
    // - Normally valid but disabled value
    // - Check what error messages get returned in each of the above cases
    // - Missing CSRF token
    // - Invalid CSRF token
    // - Re-used CSRF token



    public class FakeLookupsReadServiceThreePhases : FakeNotImplementedLookupsReadService
    {
        public override Task<IEnumerable<Phase>> GetPhases()
        {
            return Task.FromResult(SamplePhases);
        }
    }

}
