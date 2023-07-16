using Application.Interfaces.Indexing;
using NSubstitute;
using Search;

namespace UnitTests.Builders;

internal class IndexingClientBuilder
{
    internal readonly IMicroshopIndex _microshopIndex;

    public IndexingClientBuilder()
    {
        _microshopIndex = Substitute.For<IMicroshopIndex>();
    }

    public IndexingClientBuilder WithIndexGetAllDocumentsReturns<T>(IEnumerable<T> items)
    {
        _microshopIndex.GetAllDocumentsAsync<T>().Returns(Task.FromResult(items));

        return this;
    }

    public IndexingClient Build() => new(_microshopIndex);
}
