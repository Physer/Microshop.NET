using Application.Authentication;
using Authentication;
using NSubstitute;

namespace UnitTests.Tokens;

internal class TokenParserBuilder
{
    private readonly ITokenHandler _tokenHandler;

    public TokenParserBuilder() => _tokenHandler = Substitute.For<ITokenHandler>();

    public TokenParserBuilder WithTokenHandlerReturning(Token token)
    {
        _tokenHandler.ReadJwt(Arg.Any<string>()).Returns(token);

        return this;
    }

    public TokenParser Build() => new(_tokenHandler);
}
