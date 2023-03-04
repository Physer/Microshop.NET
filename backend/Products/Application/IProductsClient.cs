using Domain;

namespace Application;

public interface IProductsClient
{
    Task<IEnumerable<Product>> GetProductsAsync();
}
