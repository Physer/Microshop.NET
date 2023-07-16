using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using UnitTests.Builders;
using Xunit;

namespace UnitTests;

public class IndexingClientTests
{
    [Theory]
    [AutoData]
    public async Task AddOrUpdateDocumentsAsync_CallsIndex(IEnumerable<object> objects)
    {
        // Arrange
        var indexingClientBuilder = new IndexingClientBuilder();
        var indexingClient = indexingClientBuilder.Build();

        // Act
        await indexingClient.AddOrUpdateDocumentsAsync(objects);

        // Assert
        await indexingClientBuilder._microshopIndex.Received(1).AddOrUpdateDocumentsAsync(objects);
    }

    [Theory]
    [AutoData]
    public async Task GetDocumentsAsync_ReturnsDocuments(IEnumerable<object> objects)
    {
        // Arrange
        var indexingClientBuilder = new IndexingClientBuilder();
        var indexingClient = indexingClientBuilder.WithIndexGetAllDocumentsReturns(objects).Build();

        // Act
        var result = await indexingClient.GetAllDocumentsAsync<object>();

        // Assert
        result.Should().BeAssignableTo<IEnumerable<object>>();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(objects.Count());
    }
}
