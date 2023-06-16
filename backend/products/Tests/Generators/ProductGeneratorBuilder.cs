using Generator;

namespace Tests.Generators;

internal class ProductGeneratorBuilder
{
    private readonly ProductGenerator _generator;

    public ProductGeneratorBuilder()
    {
        _generator = new ProductGenerator();
    }

    public ProductGenerator Build() => _generator;
}
