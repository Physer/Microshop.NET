using Domain;

namespace Persistence;

public class ProductDataMapper : IProductMapper
{
    public IEnumerable<ProductData> MapProductsToDatabaseEntries(IEnumerable<Product> products) => products.Select(MapProductToDatabaseEntry);
    public ProductData MapProductToDatabaseEntry(Product product) => new() { Description = product.Description, Id = Guid.NewGuid(), Name = product.Name, ProductCode = product.ProductCode };
    public IEnumerable<Product> MapDatabaseEntriesToProducts(IEnumerable<ProductData> productData) => productData.Select(MapDatabaseEntryToProduct);
    public Product MapDatabaseEntryToProduct(ProductData productData) => new() { Description = productData.Description, Name = productData.Name, ProductCode = productData.ProductCode };
}
