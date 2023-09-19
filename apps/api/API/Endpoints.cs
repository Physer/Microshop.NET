using Application.Interfaces.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace API;

public static class Endpoints
{
    [Authorize]
    public static async Task<IResult> GenerateProducts([FromServices] IGenerateProductsPublisher publisher)
    {
        await publisher.PublishMessage();
        return Results.Accepted();
    }

    [Authorize]
    public static async Task<IResult> ClearData([FromServices] IClearDataPublisher publisher)
    {
        await publisher.PublishMessage();
        return Results.Accepted();
    }
}
