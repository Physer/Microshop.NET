using Application;
using Domain;

namespace Persistence;

public class ProductRepository : IRepository
{
    private HashSet<ProductData> _databaseProducts = new()
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
        if (databaseProduct is null)
            return null;

        return new Product
        {
            Description = databaseProduct.Description,
            Name = databaseProduct.Name,
            ProductCode = databaseProduct.ProductCode
        };
    }

    public IEnumerable<Product> GetProducts() =>
        _databaseProducts.Select(entry => new Product
        {
            Description = entry.Description,
            Name = entry.Name,
            ProductCode = entry.ProductCode
        });
}
