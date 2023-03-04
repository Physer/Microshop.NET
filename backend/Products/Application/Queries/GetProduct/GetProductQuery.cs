using Domain;
using MediatR;

namespace Application.Queries.GetProduct;

public readonly record struct GetProductQuery(Guid ProductId) : IRequest<Product>;
