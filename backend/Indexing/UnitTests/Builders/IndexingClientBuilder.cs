using Application.Interfaces.Indexing;
using NSubstitute;
using Search;

namespace Tests.Builders;

internal class IndexingClientBuilder
{
    internal readonly IMicroshopIndex _microshopIndex;

    public IndexingClientBuilder()
    {
        _microshopIndex = Substitute.For<IMicroshopIndex>();
    }

    public static IndexingClient Build() => new();
}
