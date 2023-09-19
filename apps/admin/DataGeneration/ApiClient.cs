using Application;
using Application.Authentication;
using Application.DataGeneration;
using Application.Exceptions;
using System.Net.Http.Headers;

namespace DataGeneration;

internal class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ITokenRetriever _tokenRetriever;

    public ApiClient(HttpClient httpClient, 
        ITokenRetriever tokenRetriever)
    {
        _httpClient = httpClient;
        _tokenRetriever = tokenRetriever;
    }

    public async Task GenerateProducts()
    {
        var accessToken = _tokenRetriever.GetAccessTokenFromCookie();
        var request = new HttpRequestMessage(HttpMethod.Post, "/data");
        request.Headers.Authorization = new AuthenticationHeaderValue(Globals.Http.BearerAuthenticationScheme, accessToken);
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new MicroshopApiException(response.ReasonPhrase);
    }
}
