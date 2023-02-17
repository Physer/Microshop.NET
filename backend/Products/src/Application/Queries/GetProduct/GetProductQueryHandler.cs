﻿using MediatR;

namespace Application.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductResponse?>
{
    private readonly IRepository _repository;

    public GetProductQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public Task<ProductResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = _repository.GetProductById(request.ProductId);
        if (product is null)
            return Task.FromResult<ProductResponse?>(null);

        var response = new ProductResponse
        {
            Description = product.Description,
            Name = product.Name
        };
        return Task.FromResult<ProductResponse?>(response);
    }
}
