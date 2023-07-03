using Application.Interfaces.Generator;
using Application.Interfaces.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductsGenerator;

var builder = Host.CreateApplicationBuilder(args);
ServiceConfigurator.ConfigureServices(builder.Configuration, builder.Services);
using var host = builder.Build();
await host.StartAsync();

var amountOfProductsToGenerate = 500;
var productGenerator = host.Services.GetRequiredService<IProductGenerator>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Generating {amountOfProductsToGenerate} fake products", amountOfProductsToGenerate);
var products = productGenerator.GenerateProducts(amountOfProductsToGenerate);

logger.LogInformation("Sending event that all products have been generated");
var messagePublisher = host.Services.GetRequiredService<IProductsGeneratedMessagePublisher>();
await messagePublisher.PublishMessage(products);

await host.StopAsync();