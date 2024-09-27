using Microshop.Library;
using System.Net;

namespace Tests.Utilities;

internal class TestableHttpClientBuilder : HttpClientBuilder<TestableHttpClientBuilder>
{
    public HttpStatusCode StatusCode => _statusCode;
    public object ResponseContent => _responseContent;
    public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => _headers;
}
