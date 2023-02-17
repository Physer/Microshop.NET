using MediatR;

namespace Application.Queries.GetProduct;

public class GetProductQuery : IRequest<ProductResponse>
{
    public required Guid ProductId { get; init; }
}
