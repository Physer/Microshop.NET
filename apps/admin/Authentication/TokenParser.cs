using Application.Authentication;
using Application.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Authentication;

internal class TokenParser : ITokenParser
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public IEnumerable<string> GetRoles(string accessToken)
    {
        var securityToken = _jwtSecurityTokenHandler.ReadJwtToken(accessToken) ?? throw new AuthenticationException("Invalid security token");
        var rolesClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type.Equals("st-role")) ?? throw new AuthenticationException("Invalid role claim");
        var parsedRolesClaim = JsonSerializer.Deserialize<MicroshopRoleClaim>(rolesClaim.Value) ?? throw new AuthenticationException("Unable to parse to Microshop.NET's role claim");
        return parsedRolesClaim.Roles;
    }

    private class MicroshopRoleClaim
    {
        [JsonPropertyName("t")]
        public required long AcquiredAt { get; init; }
        [JsonPropertyName("v")]
        public required IEnumerable<string> Roles { get; init; }
    }
}
