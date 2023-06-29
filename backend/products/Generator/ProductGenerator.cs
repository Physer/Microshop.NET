using Application.Interfaces.Generator;
using Application.Options;
using AutoBogus;
using Bogus;
using Domain;
using Microsoft.Extensions.Options;

namespace Generator;

public class ProductGenerator : IProductGenerator
{
    private readonly Faker<Product> _productFaker;

    public ProductGenerator(IOptions<DataOptions> options)
    {
        var seed = options?.Value?.Seed;
        if (seed is not null && seed.Value != 0)
            Randomizer.Seed = new Random(seed.Value);

        _productFaker = new AutoFaker<Product>()
            .RuleFor(fake => fake.ProductCode, fake => fake.Commerce.Ean13())
            .RuleFor(fake => fake.Name, fake => fake.Commerce.ProductName())
            .RuleFor(fake => fake.Description, fake => fake.Commerce.ProductDescription());
    }

    public IEnumerable<Product> GenerateProducts(int amountToGenerate) => _productFaker.Generate(amountToGenerate);
}
