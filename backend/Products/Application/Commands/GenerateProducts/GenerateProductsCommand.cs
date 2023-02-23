using MediatR;

namespace Application.Commands.GenerateProducts;

public readonly record struct GenerateProductsCommand(int AmountToGenerate) : IRequest;
