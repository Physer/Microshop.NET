using Application;
using Domain;

namespace Persistence;

public class ProductRepository : IRepository
{
    public IEnumerable<Product> GetProducts()
    {
        var fakeProducts = new[]
        {
            new ProductData
            {
                Description= "Test",
                Name = "Test",
                ProductCode = "Test",
            },
            new ProductData
            {
                Description= "Test2",
                Name = "Test2",
                ProductCode = "Test2",
            },
        };

        return fakeProducts.Select(fake => new Product
        {
            Description = fake.Description,
            Name = fake.Name,
            ProductCode = fake.ProductCode
        });
    }
}
