﻿using Application.Interfaces.Indexing;

namespace Search;

public class IndexingClient : IIndexingClient
{
    public async Task DeleteAllDocumentsAsync(IMicroshopIndex index) => await index.DeleteAllDocumentsAsync();
    public async Task AddDocumentsAsync<T>(IMicroshopIndex index, IEnumerable<T> documentsToIndex) => await index.AddDocumentsAsync(documentsToIndex);
}
