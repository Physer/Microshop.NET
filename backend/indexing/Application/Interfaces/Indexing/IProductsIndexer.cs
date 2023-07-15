using Domain;

namespace Application.Interfaces.Indexing;

public interface IProductsIndexer
{
    Task IndexProductsAsync(IEnumerable<Product> products);
}
