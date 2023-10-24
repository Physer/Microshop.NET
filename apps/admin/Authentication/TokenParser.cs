using Application.Authentication;
using Application.Exceptions;
using Authentication.Models;
using System.Text.Json;

namespace Authentication;

internal class TokenParser : ITokenParser
{
    private readonly ITokenHandler _tokenHandler;

    public TokenParser(ITokenHandler tokenHandler) => _tokenHandler = tokenHandler;

    public IEnumerable<string> GetRoles(string accessToken)
    {
        var securityToken = _tokenHandler.ReadJwt(accessToken);
        var rolesClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type.Equals("st-role")) ?? throw new AuthenticationException("Invalid role claim");
        var parsedRolesClaim = JsonSerializer.Deserialize<MicroshopRoleClaim>(rolesClaim.Value)!;
        return parsedRolesClaim.Roles;
    }
}
