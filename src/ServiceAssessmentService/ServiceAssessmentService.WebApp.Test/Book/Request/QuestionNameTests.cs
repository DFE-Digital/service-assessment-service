using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Book;
using ServiceAssessmentService.WebApp.Test.TestHelpers;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test.Book.Request;

public class QuestionNameTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly BookingRequestId ArbitraryBookingRequestId = new(new Guid("4AC3BA18-D2E6-43F9-AFB8-67CE72DC81E8"));
    private static string Url => $"/Book/BookingRequest/{ArbitraryBookingRequestId}/Name";

    private readonly WebApplicationFactory<Program> _factory;

    public QuestionNameTests(WebApplicationFactory<Program> factory)
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



    private WebApplicationFactory<Program> ClientWithFakeApiReturningNoRequest => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IBookingRequestReadService, FakeBookingRequestReadServiceReturningNoRequest>();
            });
        });

    private WebApplicationFactory<Program> ClientWithFakeApiReturningArbitraryDiscoveryRequest => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IBookingRequestReadService, FakeBookingRequestReadServiceReturningArbitraryDiscoveryRequest>();
            });
        });

    private WebApplicationFactory<Program> ClientWithFakeApiReturningArbitraryAlphaRequest => _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IBookingRequestReadService, FakeBookingRequestReadServiceReturningArbitraryAlphaRequest>();
            });
        });



    [Fact]
    public async Task WhenRequestNotFound_Then_Http404NotFound()
    {
        // Arrange
        var client = ClientWithFakeApiReturningNoRequest.CreateClient();

        // Act
        var response = await client.GetAsync(Url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task WhenDiscovery_Then_QuestionNameRefersToDiscovery()
    {
        // Arrange
        var client = ClientWithFakeApiReturningArbitraryDiscoveryRequest.CreateClient();

        // Act
        var response = await client.GetAsync(Url);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        document.QuerySelector("h1").TextContent
            .Should().Be("What is the name of your discovery?");
    }

    [Fact]
    public async Task WhenDiscovery_Then_QuestionHintTextIsCorrect()
    {
        // Arrange
        var client = ClientWithFakeApiReturningArbitraryDiscoveryRequest.CreateClient();

        // Act
        var response = await client.GetAsync(Url);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        document.QuerySelector(".govuk-hint").TextContent
            .Should().Be("This can be changed in the future if you develop a service.");
    }

    [Fact]
    public async Task WhenAlpha_Then_QuestionNameRefersToService()
    {
        // Arrange
        var client = ClientWithFakeApiReturningArbitraryAlphaRequest.CreateClient();

        // Act
        var response = await client.GetAsync(Url);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        document.QuerySelector("h1").TextContent
            .Should().Be("What is the name of your service?");
    }

    [Fact]
    public async Task WhenAlpha_Then_MustBeNoQuestionHintText()
    {
        // Arrange
        var client = ClientWithFakeApiReturningArbitraryAlphaRequest.CreateClient();

        // Act
        var response = await client.GetAsync(Url);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert - expect no hint text
        document.QuerySelector(".govuk-hint")
            .Should().BeNull();
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
