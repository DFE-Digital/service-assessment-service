using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test;

public class BasicWebTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicWebTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public static IEnumerable<object[]> SimpleUrls =>
        new List<object[]>
        {
            new object[] { "/" },
            new object[] { "/AccessibilityStatement" },
            new object[] { "/CookiePolicy" },
            new object[] { "/Privacy" },
            new object[] { "/Dashboard" },
            new object[] { "/Book/BookingRequest/Index" },
            // new object[] { "/Book/Request/AssessmentType/Index" },
        };

    [Theory]
    [MemberData(nameof(SimpleUrls))]
    public async Task StatusCodeMustIndicateSuccessResponse(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

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

    [Theory]
    [MemberData(nameof(SimpleUrls))]
    public async Task ContentTypeMustBeTextHtmlUtf8(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Content?.Headers?.ContentType?.ToString()
            .Should().Be("text/html; charset=utf-8");
    }
}
