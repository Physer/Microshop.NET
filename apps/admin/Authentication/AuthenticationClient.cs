using Application.Authentication;
using Application.Exceptions;
using Authentication.Models;
using System.Text.Json;

namespace Authentication;

internal class AuthenticationClient : IAuthenticationClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public AuthenticationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<AuthenticationData> SigIn(string username, string password)
    {
        var requestData = AuthenticationMapper.MapToRequest(username, password);
        var serializedRequestData = JsonSerializer.Serialize(requestData, _serializerOptions);
        var response = await _httpClient.PostAsync("/auth/signin", new StringContent(serializedRequestData));
        if (!response.IsSuccessStatusCode)
            throw new AuthenticationException();

        var parsedResponse = AuthenticationMapper.MapFromResponse(await response.Content.ReadAsStringAsync(), _serializerOptions);
        if (parsedResponse.Status != AuthenticationStatus.OK)
            throw new AuthenticationException();

        if(!response.Headers.TryGetValues("st-access-token", out var accessTokenData) || accessTokenData?.Any() == false)
            throw new AuthenticationException();

        if(!response.Headers.TryGetValues("st-refresh-token", out var refreshTokenData) || refreshTokenData?.Any() == false)
            throw new AuthenticationException();

        return new(accessTokenData!.First(), refreshTokenData!.First());
    }
}
