using Application.Authentication;
using Authentication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Authentication;

internal class AuthenticationClient : IAuthenticationClient
{
    private readonly HttpClient _httpClient;

    public AuthenticationClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<SignInResult> SignInAsync(string username, string password)
    {
        var signInRequest = new SignInRequest(username, password);
        var serializedSignInRequest = JsonSerializer.Serialize(signInRequest, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var response = await _httpClient.PostAsync("/auth/signin", new StringContent(serializedSignInRequest));
        if (!response.Headers.TryGetValues("st-access-token", out var accessTokenResponse) && accessTokenResponse?.Any() == false)
            return new();

        var accessToken = accessTokenResponse is not null ? accessTokenResponse.FirstOrDefault() : string.Empty;
        return string.IsNullOrWhiteSpace(accessToken) ? new() : new(AccessTokenHasAdminRights(accessToken), accessToken);
    }

    private static bool AccessTokenHasAdminRights(string token)
    {
        var foo = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var rolesClaim = foo.Claims.FirstOrDefault(claim => claim.Type.Equals("st-role", StringComparison.InvariantCultureIgnoreCase));
        if (rolesClaim is null)
            return false;

        var roles = JsonSerializer.Deserialize<RolesClaimModel>(rolesClaim.Value, new JsonSerializerOptions(JsonSerializerDefaults.Web))?.Roles;
        return roles is not null && roles.Any(role => role.Equals("admin", StringComparison.InvariantCultureIgnoreCase));
    }
}
