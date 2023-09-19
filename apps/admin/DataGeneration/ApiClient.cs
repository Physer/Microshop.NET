using Application.DataGeneration;
using Application.Exceptions;

namespace DataGeneration;

internal class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task GenerateProducts()
    {
        var response = await _httpClient.PostAsync("/products", new StringContent(string.Empty));
        if (!response.IsSuccessStatusCode)
            throw new MicroshopApiException(response.ReasonPhrase);
    }
}
