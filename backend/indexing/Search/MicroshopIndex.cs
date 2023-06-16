﻿using Application.Interfaces.Indexing;
using Meilisearch;
using System.Diagnostics.CodeAnalysis;
using Index = Meilisearch.Index;

namespace Search;

[ExcludeFromCodeCoverage]
public class MicroshopIndex : IMicroshopIndex
{
    private readonly Index _index;

    public MicroshopIndex(MeilisearchClient meilisearchClient) => _index = meilisearchClient.Index("products");

    public async Task DeleteAllDocumentsAsync() => await _index.DeleteAllDocumentsAsync();

    public async Task AddDocumentsAsync<T>(IEnumerable<T> documentsToIndex) => await _index.AddDocumentsAsync(documentsToIndex);
    public async Task<IEnumerable<T>> GetAllDocumentsAsync<T>() => (await _index.GetDocumentsAsync<T>(new Meilisearch.QueryParameters.DocumentsQuery { Offset = 0, Limit = int.MaxValue })).Results;
}