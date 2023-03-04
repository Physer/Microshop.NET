using Application.Models;

namespace Application.Interfaces.ProductsClient;

public interface IProductsClient
{
    Task<IEnumerable<ProductResponse>> GetProductsAsync();
}
