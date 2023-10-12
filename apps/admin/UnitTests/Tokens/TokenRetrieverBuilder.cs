using Application.Authentication;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace UnitTests.Tokens;

internal class TokenRetrieverBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenRetrieverBuilder() => _httpContextAccessor = Substitute.For<IHttpContextAccessor>();

    public TokenRetrieverBuilder WithAuthorizationCookieData(string cookieName, string cookieValue)
    {
        _httpContextAccessor.HttpContext?.Request.Cookies.Returns(new FakeRequestCookieCollection(cookieName, cookieValue));

        return this;
    }

    public TokenRetriever Build() => new(_httpContextAccessor);
}
