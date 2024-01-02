namespace ServiceAssessmentService.WebApp.Test.TestHelpers;
using System.Net.Http.Headers;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

// See also: https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers/HtmlHelpers.cs
// See also: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public static class HtmlHelpers
{
    public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var document = await BrowsingContext.New()
            .OpenAsync(ResponseFactory, CancellationToken.None);
        return (IHtmlDocument)document;

        void ResponseFactory(VirtualResponse htmlResponse)
        {
            htmlResponse
                .Address(response.RequestMessage.RequestUri)
                .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);

            htmlResponse.Content(content);

            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }
}
