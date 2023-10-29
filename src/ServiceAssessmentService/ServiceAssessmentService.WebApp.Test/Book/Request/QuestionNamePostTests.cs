using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ServiceAssessmentService.WebApp.Controllers.Book;
using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Book;
using ServiceAssessmentService.WebApp.Test.TestHelpers;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test.Book.Request;

/*
 * Note that the purpose of tests within this test class is to confirm that the page displays content appropriate to the API responses.
 * The web app is a thin-client and should display errors per what the API returns.
 * This means that tests covering specific validation rules should be run against the API, not the web app.
 */
public class QuestionNamePostTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ArbitraryErrorMessageText = "test error message";
    private static readonly BookingRequestId ArbitraryBookingRequestId = new(new Guid("4AC3BA18-D2E6-43F9-AFB8-67CE72DC81E8"));
    private static string Url => $"/Book/BookingRequest/{ArbitraryBookingRequestId}/Name";

    private readonly WebApplicationFactory<Program> _factory;

    public QuestionNamePostTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }



    private static readonly IncompleteBookingRequest ArbitraryIncompleteDiscoveryBookingRequest = new(ArbitraryBookingRequestId)
    {
        PhaseConcluding = new Phase() { Id = "discovery", DisplayNameLowerCase = "discovery" }
    };

    private static readonly IncompleteBookingRequest ArbitraryIncompleteAlphaBookingRequest = new(ArbitraryBookingRequestId)
    {
        PhaseConcluding = new Phase() { Id = "alpha", DisplayNameLowerCase = "alpha" }
    };



    private WebApplicationFactory<Program> ClientWithFakeApiReturningOneError => _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IBookingRequestReadService, FakeBookingRequestReadServiceReturningArbitraryDiscoveryRequest>();
            services.AddScoped<IBookingRequestWriteService, FakeBookingRequestWriteServiceReturningOneError>();
        });
    });

    private WebApplicationFactory<Program> ClientWithFakeApiReturningNoErrors => _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IBookingRequestReadService, FakeBookingRequestReadServiceReturningArbitraryDiscoveryRequest>();
            services.AddScoped<IBookingRequestWriteService, FakeBookingRequestWriteServiceReturningSuccessNoErrors>();
        });
    });


    [Fact]
    public async Task GivenApiResponseIndicatingSuccess_THEN_ResponseCodeMustBeRedirect()
    {
        // Arrange
        var client = ClientWithFakeApiReturningNoErrors
            .CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    // On form submission, should receive POST and redirect -- do not follow this redirect
                    AllowAutoRedirect = false,
                }
            );

        // Must perform post after a get, to first generate/fetch/include the CSRF token (used within the subsequent post)
        var responseGet = await client.GetAsync(Url);
        responseGet.EnsureSuccessStatusCode();

        var contentGet = await HtmlHelpers.GetDocumentAsync(responseGet);

        // Get the (single) form/submit button from within the page body (ignoring, e.g., forms within the header/footer).
        // If more than one is found, this is a problem and should be flagged as an error.
        var mainPageContentSection = contentGet.QuerySelectorAll(".main--content").Single();
        var formElement = mainPageContentSection.QuerySelectorAll<IHtmlFormElement>("form").Single();
        var submitButtonElement = formElement.QuerySelectorAll<IHtmlButtonElement>("button[type='submit']").Single();

        // Set a valid value
        var textInputElement = formElement[AssessmentNameDto.FormName] as IHtmlInputElement;
        textInputElement.Should().NotBeNull();
        textInputElement.Value = "abc";


        // Act - submit the form
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


    [Fact]
    public async Task GivenApiResponseIndicatingOneError_THEN_ResponseCodeMustBe422()
    {
        // Arrange
        var client = ClientWithFakeApiReturningOneError
            .CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    // On form submission, should receive POST and redirect -- do not follow this redirect
                    AllowAutoRedirect = false,
                }
            );

        // Must perform post after a get, to first generate/fetch/include the CSRF token (used within the subsequent post)
        var responseGet = await client.GetAsync(Url);
        responseGet.EnsureSuccessStatusCode();

        var contentGet = await HtmlHelpers.GetDocumentAsync(responseGet);

        // Get the (single) form/submit button from within the page body (ignoring, e.g., forms within the header/footer).
        // If more than one is found, this is a problem and should be flagged as an error.
        var mainPageContentSection = contentGet.QuerySelectorAll(".main--content").Single();
        var formElement = mainPageContentSection.QuerySelectorAll<IHtmlFormElement>("form").Single();
        var submitButtonElement = formElement.QuerySelectorAll<IHtmlButtonElement>("button[type='submit']").Single();

        // Set a valid value
        var textInputElement = formElement[AssessmentNameDto.FormName] as IHtmlInputElement;
        textInputElement.Should().NotBeNull();
        textInputElement.Value = "abc";


        // Act - submit the form
        var postResponse = await client.SendAsync(formElement, submitButtonElement);

        // Assert - Expect form submission to be rejected
        var statusCode = (int)postResponse.StatusCode;
        statusCode.Should().Be(422);
    }


    [Fact]
    public async Task GivenApiResponseIndicatingOneError_THEN_ErrorMessageMustBeDisplayed()
    {
        // Arrange
        var client = ClientWithFakeApiReturningOneError
            .CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    // On form submission, should receive POST and redirect -- do not follow this redirect
                    AllowAutoRedirect = false,
                }
            );

        // Must perform post after a get, to first generate/fetch/include the CSRF token (used within the subsequent post)
        var responseGet = await client.GetAsync(Url);
        responseGet.EnsureSuccessStatusCode();

        var contentGet = await HtmlHelpers.GetDocumentAsync(responseGet);

        // Get the (single) form/submit button from within the page body (ignoring, e.g., forms within the header/footer).
        // If more than one is found, this is a problem and should be flagged as an error.
        var mainPageContentSection = contentGet.QuerySelectorAll(".main--content").Single();
        var formElement = mainPageContentSection.QuerySelectorAll<IHtmlFormElement>("form").Single();
        var submitButtonElement = formElement.QuerySelectorAll<IHtmlButtonElement>("button[type='submit']").Single();

        // Set a valid value
        var textInputElement = formElement[AssessmentNameDto.FormName] as IHtmlInputElement;
        textInputElement.Should().NotBeNull();
        textInputElement.Value = "abc";


        // Act - submit the form
        var postResponse = await client.SendAsync(formElement, submitButtonElement);
        var document = await HtmlHelpers.GetDocumentAsync(postResponse);

        // Assert - Expect error messages to be displayed twice -- once within summary, once next to field
        // Depending on exact markup, may be spurious whitespace present, so normalise whitespace before comparison
        using (new AssertionScope())
        {
            // Error message next to field -- should be single element with error message prefixed with visually-hidden "Error: "
            var element = document.QuerySelectorAll($"#{AssessmentNameDto.FormName}-error").Single();
            element.Should().NotBeNull();
            TextHelper.NormaliseWhitespace(element.TextContent)
                .Should().Be(TextHelper.NormaliseWhitespace($"Error: {ArbitraryErrorMessageText}"));

            // Error summary - should be single <li> with error message as text content
            var summaryElement = document.QuerySelectorAll(".govuk-error-summary").Single();
            summaryElement.Should().NotBeNull();
            var summaryLi = document.QuerySelectorAll(".govuk-error-summary__list li").Single();
            summaryLi.Should().NotBeNull();
            TextHelper.NormaliseWhitespace(summaryLi.TextContent)
                .Should().Be(TextHelper.NormaliseWhitespace(ArbitraryErrorMessageText));
        }
    }




    public class FakeBookingRequestWriteServiceReturningSuccessNoErrors : FakeNotImplementedBookingRequestWriteService
    {
        public override Task<ChangeRequestModel> UpdateRequestName(BookingRequestId id, string proposedName)
        {
            return Task.FromResult(new ChangeRequestModel()
            {
                IsSuccessful = true,
                Errors = new List<ChangeRequestModel.Error>(),
            });
        }
    }

    public class FakeBookingRequestWriteServiceReturningOneError : FakeNotImplementedBookingRequestWriteService
    {
        public override Task<ChangeRequestModel> UpdateRequestName(BookingRequestId id, string proposedName)
        {
            return Task.FromResult(new ChangeRequestModel()
            {
                IsSuccessful = false,
                Errors = new List<ChangeRequestModel.Error>()
                {
                    new(ArbitraryErrorMessageText),
                },
            });
        }
    }

    public class FakeBookingRequestReadServiceReturningNoRequest : FakeNotImplementedBookingRequestReadService
    {
        public override Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
        {
            return Task.FromResult<IncompleteBookingRequest?>(null);
        }
    }

    public class FakeBookingRequestReadServiceReturningArbitraryDiscoveryRequest : FakeNotImplementedBookingRequestReadService
    {
        public override Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
        {
            return Task.FromResult<IncompleteBookingRequest?>(ArbitraryIncompleteDiscoveryBookingRequest);
        }
    }

    public class FakeBookingRequestReadServiceReturningArbitraryAlphaRequest : FakeNotImplementedBookingRequestReadService
    {
        public override Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
        {
            return Task.FromResult<IncompleteBookingRequest?>(ArbitraryIncompleteAlphaBookingRequest);
        }
    }



}
