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
        var response = await _httpClient.PostAsync("/auth/signup", new StringContent(JsonSerializer.Serialize(signInRequest)));

        return new(false, default);
    }
}
