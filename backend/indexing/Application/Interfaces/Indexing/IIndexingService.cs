using Domain;

namespace Application.Interfaces.Indexing;

public interface IIndexingService
{
    Task IndexProductsAsync(IEnumerable<Product> products);
}
