using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp;
using System.Net.Http.Headers;

namespace IntegrationTests.Utilities;

/// <summary>
/// HTML Helpers to do integration testing in Razor pages
/// See: https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers/HtmlHelpers.cs
/// </summary>
internal static class HtmlHelpers
{
    public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response?.RequestMessage, nameof(response));

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
