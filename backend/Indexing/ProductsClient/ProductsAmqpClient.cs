using Application.Interfaces.ProductsClient;
using Domain;

namespace ProductsClient;

public class ProductsAmqpClient : IProductsClient
{
    public Task<IEnumerable<Product>> GetProductsAsync()
    {
        throw new NotImplementedException();
    }
}
