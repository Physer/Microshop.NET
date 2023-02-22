using Application;
using AutoBogus;
using Bogus;
using Domain;

namespace Generator;

public class ProductGenerator : IProductGenerator
{
    private readonly Faker<Product> _productFaker;

    public ProductGenerator()
    {
        _productFaker = new AutoFaker<Product>()
            .RuleFor(fake => fake.ProductCode, fake => fake.Commerce.Ean13())
            .RuleFor(fake => fake.Name, fake => fake.Commerce.ProductName())
            .RuleFor(fake => fake.Description, fake => fake.Commerce.ProductDescription());
    }

    public IEnumerable<Product> GenerateProducts(int amountToGenerate) => _productFaker.Generate(amountToGenerate);
}
