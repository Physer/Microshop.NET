using Application;
using Domain;

namespace Persistence;

public class ProductRepository : IRepository
{
    private readonly HashSet<ProductData> _databaseProducts = new()
    {
        new ProductData
        {
            Description = "A product description",
            Name = "Productus marvelous",
            ProductCode = "978020137962",
            Id = Guid.Empty,
        }
    };

    public Product? GetProductById(Guid id)
    {
        var databaseProduct = _databaseProducts.FirstOrDefault(entry => entry.Id.Equals(id));
        return databaseProduct is not null ? MapDatabaseEntryToProduct(databaseProduct) : null;
    }

    public IEnumerable<Product> GetProducts() => _databaseProducts.Select(MapDatabaseEntryToProduct);

    public void CreateProduct(Product product) => _databaseProducts.Add(MapProductToDatabaseEntry(product));

    public void CreateProducts(IEnumerable<Product> products) => _databaseProducts.UnionWith(products.Select(MapProductToDatabaseEntry));

    private ProductData MapProductToDatabaseEntry(Product product) => new() { Description = product.Description, Id = Guid.NewGuid(), Name = product.Name, ProductCode = product.ProductCode };
    private Product MapDatabaseEntryToProduct(ProductData productData) => new() { Description = productData.Description, Name = productData.Name, ProductCode = productData.ProductCode };
}
