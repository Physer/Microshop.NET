using Application;
using Application.Commands.GenerateProducts;
using Application.Queries.GetProduct;
using Application.Queries.GetProducts;
using AutoMapper;
using MediatR;

namespace API;

public static class ProductEndpoints
{
    public static async Task<IResult> GetAllProducts(IMediator mediator, IMapper mapper) => Results.Ok((await mediator.Send(new GetProductsQuery())).Select(mapper.Map<ProductResponse>));
    public static async Task<IResult> GetProduct(IMediator mediator, IMapper mapper, Guid id) => Results.Ok(mapper.Map<ProductResponse>(await mediator.Send(new GetProductQuery { ProductId = id })));
    public static async Task<IResult> GenerateProducts(IMediator mediator, int amountToGenerate)
    {
        await mediator.Send(new GenerateProductsCommand(amountToGenerate));
        return Results.Accepted();
    }
}
