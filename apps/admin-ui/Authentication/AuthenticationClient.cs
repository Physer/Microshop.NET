using Application.Authentication;
using Authentication.Models;
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
        return string.IsNullOrWhiteSpace(accessToken) ? new() : new(true, accessToken);
    }
}
