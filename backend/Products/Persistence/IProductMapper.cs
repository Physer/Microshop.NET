using Domain;

namespace Persistence;

public interface IProductMapper
{
    Product MapDatabaseEntryToProduct(ProductData productData);
    ProductData MapProductToDatabaseEntry(Product product);
}
