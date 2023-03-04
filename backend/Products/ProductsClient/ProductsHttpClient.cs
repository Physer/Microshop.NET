using Application.Interfaces.ProductsClient;
using Application.Models;
using System.Net.Http.Json;

namespace ProductsClient;

public class ProductsHttpClient : IProductsClient
{
    private readonly HttpClient _httpClient;

    public ProductsHttpClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync() => await _httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>("/products") ?? Enumerable.Empty<ProductResponse>();
}
