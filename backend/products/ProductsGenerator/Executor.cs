using Application.Interfaces.Generator;
using Application.Interfaces.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProductsGenerator;

internal static class Executor
{
    public static async Task GenerateAsync(IHost host)
    {
        var amountOfProductsToGenerate = 500;
        var productGenerator = host.Services.GetRequiredService<IProductGenerator>();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Generating {amountOfProductsToGenerate} fake products", amountOfProductsToGenerate);
        var products = productGenerator.GenerateProducts(amountOfProductsToGenerate);

        logger.LogInformation("Sending event that all products have been generated");
        var messagePublisher = host.Services.GetRequiredService<IProductsGeneratedMessagePublisher>();
        var publishedMessageId = await messagePublisher.PublishMessage(products);
        logger.LogInformation("Sent message {messageId}", publishedMessageId);
    }
}
