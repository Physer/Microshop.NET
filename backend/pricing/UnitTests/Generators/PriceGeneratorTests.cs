using Domain;
using FluentAssertions;
using Xunit;

namespace UnitTests.Generators;

public class PriceGeneratorTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(500)]
    [InlineData(123)]
    public void GeneratePrices_WithAmountToGenerate_ReturnsGeneratedPrices(int amountToGenerate)
    {
        // Arrange
        var priceGenerator = new PriceGeneratorBuilder()
            .Build();

        // Act
        var result = priceGenerator.GeneratePrices(amountToGenerate);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(amountToGenerate);
        result.Should().AllBeOfType<Price>();
    }

    [Fact]
    public void GeneratePrices_WithSeed_ReturnsRepeatableSet()
    {
        // Arrange
        var results = new List<IEnumerable<Price>>();
        var amountOfSetsToGenerate = 5;

        // Act
        for (var i = 0; i < amountOfSetsToGenerate; i++)
        {
            var priceGenerator = new PriceGeneratorBuilder()
            .WithSeed(1000)
            .Build();
            var result = priceGenerator.GeneratePrices(100);
            results.Add(result);
        }

        // Assert
        var generatedPricesSet = results.First();
        results.Should().HaveCount(amountOfSetsToGenerate);
        results.Should().AllSatisfy(result => result.Should().BeEquivalentTo(generatedPricesSet));
    }

    [Fact]
    public void GeneratePrices_WithoutSeed_ReturnsRandomSets()
    {
        // Arrange
        var priceGenerator = new PriceGeneratorBuilder()
            .Build();
        var results = new List<IEnumerable<Price>>();
        var amountOfSetsToGenerate = 5;

        // Act
        for (var i = 0; i < amountOfSetsToGenerate; i++)
        {
            var result = priceGenerator.GeneratePrices(100);
            results.Add(result);
        }

        // Assert
        results.Should().HaveCount(amountOfSetsToGenerate);
        results.Should().AllSatisfy(result => result.Should().OnlyHaveUniqueItems());
    }

    [Fact]
    public void GeneratePrices_WithoutOptions_ReturnsRandomSets()
    {
        // Arrange
        var priceGenerator = new PriceGeneratorBuilder()
            .WithoutOptions()
            .Build();
        var results = new List<IEnumerable<Price>>();
        var amountOfSetsToGenerate = 5;

        // Act
        for (var i = 0; i < amountOfSetsToGenerate; i++)
        {
            var result = priceGenerator.GeneratePrices(100);
            results.Add(result);
        }

        // Assert
        results.Should().HaveCount(amountOfSetsToGenerate);
        results.Should().AllSatisfy(result => result.Should().OnlyHaveUniqueItems());
    }
}
