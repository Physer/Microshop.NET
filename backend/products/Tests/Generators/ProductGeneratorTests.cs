using Domain;
using FluentAssertions;

namespace Tests.Generators;

public class ProductGeneratorTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(500)]
    [InlineData(123)]
    public void GenerateProducts_WithAmountToGenerate_ReturnsGeneratedProducts(int amountToGenerate)
    {
        // Arrange
        var productGenerator = new ProductGeneratorBuilder()
            .Build();

        // Act
        var result = productGenerator.GenerateProducts(amountToGenerate);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(amountToGenerate);
        result.Should().AllBeOfType<Product>();
    }

    [Fact]
    public void GenerateProducts_WithSeed_ReturnsRepeatableSet()
    {
        // Arrange
        var results = new List<IEnumerable<Product>>();
        var amountOfSetsToGenerate = 5;

        // Act
        for (var i = 0; i < amountOfSetsToGenerate; i++)
        {
            var productGenerator = new ProductGeneratorBuilder()
            .WithSeed(1000)
            .Build();
            var result = productGenerator.GenerateProducts(100);
            results.Add(result);
        }

        // Assert
        var generatedProductsSet = results.First();
        results.Should().HaveCount(amountOfSetsToGenerate);
        results.Should().AllSatisfy(result => result.Should().BeEquivalentTo(generatedProductsSet));
    }

    [Fact]
    public void GenerateProducts_WithoutSeed_ReturnsRandomSets()
    {
        // Arrange
        var productGenerator = new ProductGeneratorBuilder()
            .Build();
        var results = new List<IEnumerable<Product>>();
        var amountOfSetsToGenerate = 5;

        // Act
        for (var i = 0; i < amountOfSetsToGenerate; i++)
        {
            var result = productGenerator.GenerateProducts(100);
            results.Add(result);
        }

        // Assert
        results.Should().HaveCount(amountOfSetsToGenerate);
        results.Should().AllSatisfy(result => result.Should().OnlyHaveUniqueItems());
    }
}
