using Application.Options;
using Generator;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace UnitTests.Generators;

internal class PriceGeneratorBuilder
{
    private IOptions<DataOptions>? _options;

    public PriceGeneratorBuilder() => _options = Substitute.For<IOptions<DataOptions>>();

    public PriceGeneratorBuilder WithoutOptions()
    {
        _options = null;

        return this;
    }

    public PriceGeneratorBuilder WithSeed(int? seed)
    {
        _options!.Value.Returns(new DataOptions
        {
            Seed = seed
        });

        return this;
    }

    public PriceGenerator Build() => new(_options!);
}
