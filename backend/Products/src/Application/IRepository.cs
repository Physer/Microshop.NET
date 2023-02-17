using Domain;

namespace Application;

public interface IRepository
{
    IEnumerable<Product> GetProducts();
    Product? GetProductById(Guid id);
}
