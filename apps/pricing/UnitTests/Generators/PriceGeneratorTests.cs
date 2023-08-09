using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using Generator;
using Xunit;

namespace UnitTests.Generators;

public class PriceGeneratorTests
{
    private readonly PriceGenerator _priceGenerator;

    public PriceGeneratorTests() => _priceGenerator = PriceGeneratorBuilder.Build();

    [Theory]
    [AutoData]
    public void GeneratePrices_WithProducts_ReturnsGeneratedPrices(IEnumerable<Product> products)
    {
        // Arrange
        // Act
        var result = _priceGenerator.GeneratePrices(products);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(products.Count());
        result.Should().AllBeOfType<Price>();
    }

    [Fact]
    public void GeneratePrices_WithEmptyProductsCollection_ReturnsEmptyCollection()
    {
        // Arrange
        // Act
        var result = _priceGenerator.GeneratePrices(default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GeneratePrices_WithoutProducts_ReturnsEmptyCollection()
    {
        // Arrange
        // Act
        var result = _priceGenerator.GeneratePrices(null);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
