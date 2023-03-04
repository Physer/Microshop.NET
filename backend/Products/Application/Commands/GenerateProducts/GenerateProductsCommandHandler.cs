using Application.Interfaces.Generator;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.GenerateProducts;

public class GenerateProductsCommandHandler : IRequestHandler<GenerateProductsCommand>
{
    private readonly IRepository _repository;
    private readonly IProductGenerator _productGenerator;

    public GenerateProductsCommandHandler(IRepository repository,
        IProductGenerator productGenerator)
    {
        _repository = repository;
        _productGenerator = productGenerator;
    }

    public async Task Handle(GenerateProductsCommand request, CancellationToken cancellationToken)
    {
        var products = _productGenerator.GenerateProducts(request.AmountToGenerate);
        await Task.Run(() => _repository.CreateProducts(products), cancellationToken);
    }
}
