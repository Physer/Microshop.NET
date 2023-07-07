using Application.Interfaces.Generator;
using Application.Interfaces.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace API;

public static class Endpoints
{
    public static async Task<IResult> GenerateProducts([FromServices] IProductGenerator generator,
        [FromServices] IProductsGeneratedMessagePublisher messagePublisher,
        [FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(nameof(GenerateProducts));
        logger.LogInformation("Generating fake products");
        var products = generator.GenerateProducts(500);
        logger.LogInformation("Sending event that all products have been generated");
        var publishedMessageId = await messagePublisher.PublishMessage(products);
        logger.LogInformation("Sent message {messageId}", publishedMessageId);
        return Results.Ok(publishedMessageId);
    }
}
