﻿using Application.Interfaces.Generator;
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
            .RuleFor(fake => fake.Value, fake => decimal.Parse(fake.Commerce.Price()))
            .RuleFor(fake => fake.Currency, _ => "EUR");
    }

    public IEnumerable<Price> GeneratePrices(IEnumerable<Product>? productData)
    {
        if (productData is null)
            yield break;

        foreach (var product in productData)
            yield return _priceFaker.RuleFor(fake => fake.ProductCode, product.Code).Generate();
    }
}
