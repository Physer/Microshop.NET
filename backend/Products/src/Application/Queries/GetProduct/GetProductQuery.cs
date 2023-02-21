using MediatR;

namespace Application.Queries.GetProduct;

public readonly record struct GetProductQuery : IRequest<ProductResponse>
{
    public required Guid ProductId { get; init; }
}
