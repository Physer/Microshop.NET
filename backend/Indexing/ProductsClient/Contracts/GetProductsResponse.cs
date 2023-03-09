namespace ProductsClient.Contracts;

public record GetProductsResponse
{
    public IEnumerable<ProductResponse> Products { get; set; } = new List<ProductResponse>();
}
