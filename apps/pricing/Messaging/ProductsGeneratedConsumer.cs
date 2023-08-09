using Application.Interfaces.Generator;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging;

public class ProductsGeneratedConsumer : IConsumer<ProductsGenerated>
{
    private readonly ILogger<ProductsGeneratedConsumer> _logger;
    private readonly IPriceGenerator _priceGenerator;

    public ProductsGeneratedConsumer(ILogger<ProductsGeneratedConsumer> logger, 
        IPriceGenerator priceGenerator)
    {
        _logger = logger;
        _priceGenerator = priceGenerator;
    }

    public async Task Consume(ConsumeContext<ProductsGenerated> context)
    {
        if (context is null)
            return;

        _logger.LogInformation("Pricing service - received message {messageId}", context?.MessageId);
        var generatedPrices = _priceGenerator.GeneratePrices(context?.Message?.Products);
        await context!.Publish<PricesGenerated>(new(generatedPrices));
    }
}
