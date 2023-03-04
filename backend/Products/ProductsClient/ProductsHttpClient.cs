using Application;
using Domain;
using System.Net.Http.Json;

namespace ProductsClient;

public class ProductsHttpClient : IProductsClient
{
    private readonly HttpClient _httpClient;

    public ProductsHttpClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IEnumerable<Product>> GetProductsAsync() => await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("/products") ?? Enumerable.Empty<Product>();
}
