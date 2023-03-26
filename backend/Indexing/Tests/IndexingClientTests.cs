using AutoFixture.Xunit2;
using NSubstitute;
using Tests.Builders;
using Xunit;

namespace Tests;

public class IndexingClientTests
{
    [Fact]
    public async Task DeletAllDocumentsAsync_CallsIndex()
    {
        // Arrange
        var indexingClientBuilder = new IndexingClientBuilder();
        var indexingClient = IndexingClientBuilder.Build();

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
        var indexingClient = IndexingClientBuilder.Build();

        // Act
        await indexingClient.AddDocumentsAsync(indexingClientBuilder._microshopIndex, objects);

        // Assert
        await indexingClientBuilder._microshopIndex.Received(1).AddDocumentsAsync(objects);
    }
}
