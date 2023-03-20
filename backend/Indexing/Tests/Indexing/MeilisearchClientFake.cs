using Meilisearch;

namespace Tests.Indexing;

internal class MeilisearchClientFake : MeilisearchClient
{
    public MeilisearchClientFake(string url, string? apiKey = null) : base(url, apiKey)
    {
    }

    public MeilisearchClientFake(HttpClient client, string? apiKey = null) : base(client, apiKey)
    {
    }

    public virtual IndexFake Index(string uid) => new(uid);
}
