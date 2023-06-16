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
        var productGenerator = new ProductGeneratorBuilder().Build();

        // Act
        var result = productGenerator.GenerateProducts(amountToGenerate);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(amountToGenerate);
        result.Should().AllBeOfType<Product>();
    }
}
