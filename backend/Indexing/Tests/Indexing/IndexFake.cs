using Index = Meilisearch.Index;

namespace Tests.Indexing;

internal class IndexFake : Index
{
    public IndexFake(string uid, string? primaryKey = null, DateTimeOffset? createdAt = null, DateTimeOffset? updatedAt = null) : base(uid, primaryKey, createdAt, updatedAt)
    {
    }

    public async Task DeleteAllDocumentsAsync() => await Task.Run(() => { });

    public async Task AddDocumentsAsync() => await Task.Run(() => { });
}
