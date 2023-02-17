using MediatR;

namespace Application.Queries.GetProducts;

public class GetProductsQuery : IRequest<IEnumerable<ProductResponse>> { }
