using Application.Interfaces.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Service;

public static class Endpoints
{
    public static async Task<IResult> GenerateProducts([FromServices] IGenerateProductsPublisher publisher)
    {
        await publisher.PublishMessage();
        return Results.Accepted();
    }
}
