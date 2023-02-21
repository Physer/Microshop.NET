using MediatR;

namespace Application.Commands.GenerateProducts;

public readonly record struct GenerateProductsCommand(int AmountToGenerate = 1000) : IRequest;
