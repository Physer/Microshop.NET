using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging;

internal class ProductsGeneratedConsumer : IConsumer<ProductsGenerated>
{
    private readonly ILogger<ProductsGeneratedConsumer> _logger;

    public ProductsGeneratedConsumer(ILogger<ProductsGeneratedConsumer> logger) => _logger = logger;

    public async Task Consume(ConsumeContext<ProductsGenerated> context)
    {
        _logger.LogInformation("Pricing service - received message {messageId}", context.MessageId);
    }
}
