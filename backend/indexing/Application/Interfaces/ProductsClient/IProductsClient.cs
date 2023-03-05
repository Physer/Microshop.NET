using Application.Models;
using Domain;

namespace Application.Interfaces.ProductsClient;

public interface IProductsClient
{
    Task<IEnumerable<Product>> GetProductsAsync();
}
