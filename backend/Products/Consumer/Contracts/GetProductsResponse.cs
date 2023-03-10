using Application.Models;

namespace ProductsClient.Contracts;

public record GetProductsResponse
{
    public GetProductsResponse(IEnumerable<ProductResponse> products)
    {
        Products = products;
    }

    public IEnumerable<ProductResponse> Products { get; init; }
}
