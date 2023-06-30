using Application.Options;
using Generator;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Tests.Generators;

internal class ProductGeneratorBuilder
{
    private readonly IOptions<DataOptions> _options;

    public ProductGeneratorBuilder()
    {
        _options = Substitute.For<IOptions<DataOptions>>();
    }

    public ProductGeneratorBuilder WithSeed(int? seed)
    {
        _options.Value.Returns(new DataOptions
        {
            Seed = seed
        });

        return this;
    }

    public ProductGenerator Build() => new(_options);
}
