using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace UnitTests;

public class IndexingClientTests
{
    [Fact]
    public async Task DeletAllDocumentsAsync_CallsIndex()
    {
        // Arrange
        var indexingClientBuilder = new IndexingClientBuilder();
        var indexingClient = indexingClientBuilder.Build();

        // Act
        await indexingClient.DeleteAllDocumentsAsync(indexingClientBuilder._microshopIndex);

        // Assert
        await indexingClientBuilder._microshopIndex.Received(1).DeleteAllDocumentsAsync();
    }

    [Theory]
    [AutoData]
    public async Task AddDocumentsAsync_CallsIndex(IEnumerable<object> objects)
    {
        // Arrange
        var indexingClientBuilder = new IndexingClientBuilder();
        var indexingClient = indexingClientBuilder.Build();

        // Act
        await indexingClient.AddDocumentsAsync(indexingClientBuilder._microshopIndex, objects);

        // Assert
        await indexingClientBuilder._microshopIndex.Received(1).AddDocumentsAsync(objects);
    }

    [Theory]
    [AutoData]
    public async Task GetDocumentsAsync_ReturnsDocuments(IEnumerable<object> objects)
    {
        // Arrange
        var indexingClientBuilder = new IndexingClientBuilder();
        var indexingClient = indexingClientBuilder.WithIndexGetAllDocumentsReturns(objects).Build();

        // Act
        var result = await indexingClient.GetAllDocumentsAsync<object>(indexingClientBuilder._microshopIndex);

        // Assert
        result.Should().BeAssignableTo<IEnumerable<object>>();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(objects.Count());
    }
}
