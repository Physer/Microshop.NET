using Domain;
using MediatR;

namespace Application.Queries.GetProduct;

public class GetProductQuery : IRequest<Product>
{
    public required Guid ProductId { get; init; }
}
