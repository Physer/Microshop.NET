using Application.Interfaces.Generator;
using MassTransit;
using Messaging.Messages;

namespace Messaging;

public class GenerateProductsConsumer : IConsumer<GenerateProducts>
{
    private readonly IProductGenerator _productGenerator;

    public GenerateProductsConsumer(IProductGenerator productGenerator) => _productGenerator = productGenerator;

    public async Task Consume(ConsumeContext<GenerateProducts> context) => await context.Publish(new ProductsGenerated(_productGenerator.GenerateProducts(500)));
}
