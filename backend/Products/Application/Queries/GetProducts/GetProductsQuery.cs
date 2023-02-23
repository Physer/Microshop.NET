using MediatR;

namespace Application.Queries.GetProducts;

public readonly record struct GetProductsQuery : IRequest<IEnumerable<ProductResponse>>;
