using Application.Authentication;
using DataManagement;
using MockHttpClient;
using NSubstitute;
using System.Net;

namespace UnitTests.DataManagement;

internal class ApiClientBuilder : HttpClientBuilder<ApiClientBuilder>
{
    private readonly ITokenRetriever _tokenRetriever;
    private readonly List<FakeHttpMessage> _fakeHttpMessages;

    public ApiClientBuilder()
    {
        _tokenRetriever = Substitute.For<ITokenRetriever>();
        _fakeHttpMessages = new();
    }

    public ApiClientBuilder WithTokenRetrieverReturningToken(string accessToken)
    {
        _tokenRetriever.GetAccessTokenFromCookie().Returns(accessToken);

        return this;
    }

    public ApiClientBuilder WithMakeRequestAsyncUsing(HttpMethod httpMethod, string requestUrl)
    {
        FakeHttpMessage fakeHttpMessage = new(HttpStatusCode.OK)
        {
            RequestUrl = requestUrl,
            HttpMethod = httpMethod
        };
        _fakeHttpMessages.Add(fakeHttpMessage);

        return this;
    }

    public ApiClient Build() => new(BuildHttpClient(_fakeHttpMessages), _tokenRetriever);
}
