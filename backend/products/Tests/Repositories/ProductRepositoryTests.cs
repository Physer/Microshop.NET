using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using Persistence;

namespace Tests.Repositories;

public class ProductRepositoryTests
{
    [Theory, AutoData]
    public void GetProductById_WithExistingId_ReturnsProduct(ProductData productData, Product output)
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder()
            .WithProductData(productData)
            .WithMappingDatabaseEntryToProductReturns(output)
            .Build();

        // Act
        var result = productRepository.GetProductById(productData.Id);

        // Assert
        result.Should().Be(output);
    }

    [Theory, AutoData]
    public void GetProductById_WithoutExistingId_ReturnsNull(Guid randomGuid)
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder().Build();

        // Act
        var result = productRepository.GetProductById(randomGuid);

        // Assert
        result.Should().BeNull();

    }

    [Fact]
    public void GetProducts_WithoutDatabaseEntries_ReturnsEmptyCollection()
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder().Build();

        // Act
        var result = productRepository.GetProducts();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void GetProducts_WithDatabaseEntries_ReturnsProducts(IEnumerable<ProductData> productData, IEnumerable<Product> products)
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder()
            .WithProductData(productData)
            .WithMappingDatabaseEntriesToProductsReturns(products)
            .Build();

        // Act
        var result = productRepository.GetProducts();

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Theory, AutoData]
    public void CreateProduct_WithProduct_AddsToDatabase(ProductData databaseEntry, Product product)
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder()
            .WithProductData(databaseEntry)
            .WithMappingDatabaseEntriesToProductsReturns(new[] { product })
            .WithMappingProductToDatabaseEntryReturns(databaseEntry)
            .Build();

        // Act
        productRepository.CreateProduct(product);

        // Assert
        var products = productRepository.GetProducts();
        products.Should().HaveCount(1);
        products.Should().Contain(product);
    }

    [Fact]
    public void CreateProducts_WithoutProducts_AddsNothingToDatabase()
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder().Build();

        // Act
        productRepository.CreateProducts(Enumerable.Empty<Product>());

        // Assert
        var products = productRepository.GetProducts();
        products.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void CreateProducts_WithProducts_AddsProductsToDatabase(IEnumerable<ProductData> databaseEntries,  IEnumerable<Product> products)
    {
        // Arrange
        var productRepository = new ProductRepositoryBuilder()
            .WithProductData(databaseEntries)
            .WithMappingProductsToDatabaseEntriesReturns(databaseEntries)
            .WithMappingDatabaseEntriesToProductsReturns(products)
            .Build();

        // Act
        productRepository.CreateProducts(products);

        // Assert
        var productsResult = productRepository.GetProducts();
        productsResult.Should().BeEquivalentTo(products);
    }
}
