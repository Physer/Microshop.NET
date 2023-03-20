using Application.Models;
using Domain;
using NSubstitute;
using Xunit;

namespace Tests.Indexing;

public class IndexingServiceTests
{
    [Fact]
    public async Task IndexProductsAsync_ShouldUseAutomapper()
    {
        // Arrange
        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder.Build();

        // Act
        await indexingService.IndexProductsAsync(CancellationToken.None);

        // Assert
        indexingServiceBuilder.MapperMock.Map<IEnumerable<IndexableProduct>>(Arg.Any<IEnumerable<Product>>()).Received(1);
    }
}
