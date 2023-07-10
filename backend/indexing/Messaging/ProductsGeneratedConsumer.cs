using Application.Interfaces.Indexing;
using Domain;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging;

internal class ProductsGeneratedConsumer : IConsumer<ProductsGenerated>
{
    private readonly IIndexingService _indexingService;
    private readonly ILogger<ProductsGeneratedConsumer> _logger;

    public ProductsGeneratedConsumer(IIndexingService indexingService,
        ILogger<ProductsGeneratedConsumer> logger)
    {
        _indexingService = indexingService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductsGenerated> context)
    {
        _logger.LogInformation("Indexing service - Received message {messageId}", context.MessageId);
        await _indexingService.IndexProductsAsync(context.Message?.Products ?? Enumerable.Empty<Product>());
    }
}
