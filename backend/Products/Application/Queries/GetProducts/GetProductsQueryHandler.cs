using Domain;
using MediatR;

namespace Application.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly IRepository _repository;

    public GetProductsQueryHandler(IRepository repository) => _repository = repository;

    public Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken _) => Task.FromResult(_repository?.GetProducts() ?? Enumerable.Empty<Product>());
}
