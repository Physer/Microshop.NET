using MediatR;

namespace Application.Commands.GenerateProducts;

public class GenerateProductsCommandHandler : IRequestHandler<GenerateProductsCommand>
{
    private readonly IRepository _repository;

    public GenerateProductsCommandHandler(IRepository repository) => _repository = repository;

    public Task Handle(GenerateProductsCommand request, CancellationToken cancellationToken) => throw new NotImplementedException();
}
