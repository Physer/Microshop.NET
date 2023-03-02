using Domain;

namespace Persistence;

public interface IProductMapper
{
    IEnumerable<Product> MapDatabaseEntriesToProducts(IEnumerable<ProductData> productData);
    Product MapDatabaseEntryToProduct(ProductData productData);
    IEnumerable<ProductData> MapProductsToDatabaseEntries(IEnumerable<Product> products);
    ProductData MapProductToDatabaseEntry(Product product);
}
