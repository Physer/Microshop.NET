using Application.Authentication;
using DataManagement;
using NSubstitute;
using UnitTests.Utilities;

namespace UnitTests.DataManagement;

internal class ApiClientBuilder : HttpClientBuilder<ApiClientBuilder>
{
    private readonly ITokenRetriever _tokenRetriever;

    public ApiClientBuilder() => _tokenRetriever = Substitute.For<ITokenRetriever>();

    public ApiClientBuilder WithTokenRetrieverReturningToken(string accessToken)
    {
        _tokenRetriever.GetAccessTokenFromCookie().Returns(accessToken);

        return this;
    }

    public ApiClient Build() => new(BuildHttpClient(), _tokenRetriever);
}
