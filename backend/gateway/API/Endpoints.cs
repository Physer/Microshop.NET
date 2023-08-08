using Application.Interfaces.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API;

public static class Endpoints
{
    [Authorize]
    public static async Task<IResult> GenerateProducts([FromServices] IGenerateProductsPublisher publisher)
    {
        await publisher.PublishMessage();
        return Results.Accepted();
    }
}
