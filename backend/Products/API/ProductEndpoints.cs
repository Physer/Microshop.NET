using Application.Commands.GenerateProducts;
using Application.Queries.GetProduct;
using Application.Queries.GetProducts;
using MediatR;

namespace API;

public static class ProductEndpoints
{
    public static async Task<IResult> GetAllProducts(IMediator mediator) => Results.Ok(await mediator.Send(new GetProductsQuery()));
    public static async Task<IResult> GetProduct(IMediator mediator, Guid id) => Results.Ok(await mediator.Send(new GetProductQuery { ProductId = id }));
    public static async Task<IResult> GenerateProducts(IMediator mediator, int amountToGenerate)
    {
        await mediator.Send(new GenerateProductsCommand(amountToGenerate));
        return Results.Accepted();
    }
}
