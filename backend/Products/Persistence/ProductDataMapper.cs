using Domain;

namespace Persistence;

public class ProductDataMapper : IProductMapper
{
    public ProductData MapProductToDatabaseEntry(Product product) => new() { Description = product.Description, Id = Guid.NewGuid(), Name = product.Name, ProductCode = product.ProductCode };
    public Product MapDatabaseEntryToProduct(ProductData productData) => new() { Description = productData.Description, Name = productData.Name, ProductCode = productData.ProductCode };
}
