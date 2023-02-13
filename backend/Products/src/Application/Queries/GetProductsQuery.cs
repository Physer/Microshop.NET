using MediatR;

namespace Application.Queries;

public class GetProductsQuery : IRequest<IEnumerable<ProductResponse>> { }
