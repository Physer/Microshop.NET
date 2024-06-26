﻿using Application.Authentication;
using Application.Exceptions;
using Authentication.Models;
using System.Text.Json;

namespace Authentication;

internal class AuthenticationClient(HttpClient httpClient,
    ITokenParser tokenParser) : IAuthenticationClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ITokenParser _tokenParser = tokenParser;

    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<AuthenticationData> SignInAsync(string username, string password)
    {
        var requestData = AuthenticationMapper.MapToRequest(username, password);
        var serializedRequestData = JsonSerializer.Serialize(requestData, _serializerOptions);
        var response = await _httpClient.PostAsync("/auth/signin", new StringContent(serializedRequestData));
        if (!response.IsSuccessStatusCode)
            throw new AuthenticationException("Invalid response received from the authentication service");

        var parsedResponse = AuthenticationMapper.MapFromResponse(await response.Content.ReadAsStringAsync());
        if (!Enum.TryParse<AuthenticationStatus>(parsedResponse.Status, out var parsedStatus) && parsedStatus != AuthenticationStatus.OK)
            throw new AuthenticationException("Unable to determine the authentication service's status result");

        if (!response.Headers.TryGetValues("st-access-token", out var accessTokenData) || accessTokenData?.Any() == false)
            throw new AuthenticationException("Unable to retrieve the access token");

        var accessToken = accessTokenData!.First();
        var roles = _tokenParser.GetRoles(accessToken);
        return new(parsedResponse.User.Email, roles, accessToken);
    }
}
