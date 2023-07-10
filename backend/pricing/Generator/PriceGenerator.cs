using Application.Interfaces.Generator;
using Application.Options;
using AutoBogus;
using Bogus;
using Domain;
using Microsoft.Extensions.Options;

namespace Generator;

internal class PriceGenerator : IPriceGenerator
{
    private readonly Faker<Price> _priceFaker;

    public PriceGenerator(IOptions<DataOptions> options)
    {
        var seed = options?.Value?.Seed;
        if (seed is not null && seed.Value != 0)
            Randomizer.Seed = new Random(seed.Value);

        _priceFaker = new AutoFaker<Price>()
            .RuleFor(fake => fake.ProductCode, fake => fake.Commerce.Ean13())
            .RuleFor(fake => fake.Value, fake => decimal.Parse(fake.Commerce.Price()));
    }

    public IEnumerable<Price> GeneratePrices(int amountToGenerate) => _priceFaker.Generate(amountToGenerate);
}
