using Application.Authentication;
using Application.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace Authentication;

internal class TokenHandler : ITokenHandler
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public TokenHandler() => _jwtSecurityTokenHandler = new();

    public Token ReadJwt(string jwt)
    {
        try
        {
            return new(_jwtSecurityTokenHandler.ReadJwtToken(jwt).Claims);
        }
        catch(Exception)
        {
            throw new AuthenticationException("Invalid security token");
        }
    }
}
