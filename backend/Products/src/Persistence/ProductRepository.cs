using Application;
using Domain;

namespace Persistence;

public class ProductRepository : IRepository
{
    private readonly IEnumerable<ProductData> _fakeProducts = new List<ProductData>
    {
        new ProductData
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Description= "Test",
                Name = "Test",
                ProductCode = "Test",
            },
            new ProductData
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Description= "Test2",
                Name = "Test2",
                ProductCode = "Test2",
            },
    };

    public Product? GetProductById(Guid id)
    {
        var fakeProduct = _fakeProducts.FirstOrDefault(fake => fake.Id.Equals(id));
        if (fakeProduct is null)
            return null;

        return new Product
        {
            Description = fakeProduct.Description,
            Name = fakeProduct.Name,
            ProductCode = fakeProduct.ProductCode
        };
    }

    public IEnumerable<Product> GetProducts() =>
        _fakeProducts.Select(fake => new Product
        {
            Description = fake.Description,
            Name = fake.Name,
            ProductCode = fake.ProductCode
        });
}
