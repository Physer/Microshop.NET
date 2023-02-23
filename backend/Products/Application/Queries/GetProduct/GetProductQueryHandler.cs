using MediatR;

namespace Application.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductResponse?>
{
    private readonly IRepository _repository;

    public GetProductQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public Task<ProductResponse?> Handle(GetProductQuery request, CancellationToken _)
    {
        var product = _repository.GetProductById(request.ProductId);
        return Task.FromResult(product is not null ? new ProductResponse { Description = product.Description, Name = product.Name} : null);
    }
}
