﻿using Application.Interfaces.Indexing;
using Domain;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging;

public class ProductsGeneratedConsumer : IConsumer<ProductsGenerated>
{
    private readonly IProductsIndexer _indexingService;
    private readonly ILogger<ProductsGeneratedConsumer> _logger;

    public ProductsGeneratedConsumer(IProductsIndexer indexingService,
        ILogger<ProductsGeneratedConsumer> logger)
    {
        _indexingService = indexingService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductsGenerated> context)
    {
        _logger.LogInformation("Indexing service - Consuming {messageId} from the {consumerName}", context?.MessageId, nameof(ProductsGeneratedConsumer));
        await _indexingService.IndexProductsAsync(context?.Message?.Products ?? Enumerable.Empty<Product>());
    }
}
