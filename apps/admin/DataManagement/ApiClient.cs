using Application;
using Application.Authentication;
using Application.DataManagement;
using Application.Exceptions;
using System.Net.Http.Headers;

namespace DataManagement;

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

    public async Task<HttpResponseMessage> GenerateProductsAsync() => _ = await MakeRequestAsync(HttpMethod.Post, "/data");

    public async Task<HttpResponseMessage> ClearDataAsync() => _ = await MakeRequestAsync(HttpMethod.Delete, "/data");

    internal async Task<HttpResponseMessage> MakeRequestAsync(HttpMethod method, string requestUrl)
    {
        var accessToken = _tokenRetriever.GetAccessTokenFromCookie();
        var request = new HttpRequestMessage(method, requestUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue(Globals.Http.BearerAuthenticationScheme, accessToken);
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new MicroshopApiException(response.ReasonPhrase);

        return response;
    }
}
