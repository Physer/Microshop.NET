﻿using Application.Interfaces.Indexing;
using Application.Interfaces.ProductsClient;
using Meilisearch;

namespace Search;

public class IndexingService : IIndexingService
{
    private readonly IProductsClient _productsClient;
    private readonly MeilisearchClient _meilisearchClient;

    public IndexingService(IProductsClient productsClient,
        MeilisearchClient meilisearchClient)
    {
        _productsClient = productsClient;
        _meilisearchClient = meilisearchClient;
    }

    public async Task IndexProductsAsync()
    {
        var products = await _productsClient.GetProductsAsync();
        var index = _meilisearchClient.Index("products");
        await index.DeleteAllDocumentsAsync();
        await index.AddDocumentsAsync(products);
    }
}
