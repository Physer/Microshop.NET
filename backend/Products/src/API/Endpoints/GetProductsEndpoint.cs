using Application;
using Application.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace API.Endpoints;

[HttpGet("/products")]
[AllowAnonymous]
public class GetProductsEndpoint : EndpointWithoutRequest<IEnumerable<ProductResponse>>
{
    private readonly IMediator _mediator;

    public GetProductsEndpoint(IMediator mediator) => _mediator = mediator;

    public override async Task<IEnumerable<ProductResponse>> ExecuteAsync(CancellationToken ct) => await _mediator.Send(new GetProductsQuery(), ct);
}
