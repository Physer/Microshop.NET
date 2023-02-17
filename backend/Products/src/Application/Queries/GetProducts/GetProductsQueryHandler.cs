using MediatR;

namespace Application.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IRepository _repository;

    public GetProductsQueryHandler(IRepository repository) => _repository = repository;

    public Task<IEnumerable<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(_repository?.GetProducts()?.Select(product => new ProductResponse { Description = product.Description, Name = product.Name }) ?? Enumerable.Empty<ProductResponse>());
}
