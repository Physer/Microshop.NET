using System.Net;

namespace UnitTests.Utilities;

internal abstract class HttpClientBuilder<T> where T : class, new()
{
    protected HttpStatusCode _statusCode;
    protected object? _responseContent;
    protected IEnumerable<KeyValuePair<string, string>>? _headers;

    public T WithResponseHavingStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;

        return this as T ?? new();
    }

    public T WithResponseHavingContent(object content)
    {
        _responseContent = content;

        return this as T ?? new();
    }

    public T WithResponseHavingHeaders(IEnumerable<KeyValuePair<string, string>> headers)
    {
        _headers = headers;

        return this as T ?? new();
    }

    protected HttpClient BuildHttpClient(IEnumerable<FakeHttpMessage?>? httpMessages = null)
    {
        if (httpMessages is null || httpMessages?.Any() == false)
        {
            httpMessages = new List<FakeHttpMessage?>
            {
                new(_statusCode, _responseContent, _headers)
            };
        }
        FakeHttpMessageHandler fakeHttpMessageHandler = new(httpMessages!);
        return new(fakeHttpMessageHandler) { BaseAddress = new Uri("http://microshop.local") };
    }
}
