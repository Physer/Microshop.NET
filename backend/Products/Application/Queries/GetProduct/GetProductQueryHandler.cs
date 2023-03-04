using Application.Interfaces.Repositories;
using Domain;
using MediatR;

namespace Application.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
{
    private readonly IRepository _repository;

    public GetProductQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public Task<Product?> Handle(GetProductQuery request, CancellationToken _) => Task.FromResult(_repository.GetProductById(request.ProductId));
}
