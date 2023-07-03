using Application.Interfaces.Generator;
using Application.Interfaces.Repositories;
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
var productRepository = host.Services.GetRequiredService<IRepository>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Generating {amountOfProductsToGenerate} fake products...", amountOfProductsToGenerate);
var products = productGenerator.GenerateProducts(amountOfProductsToGenerate);
productRepository.CreateProducts(products);
logger.LogInformation("Succesfully generated and stored product data!");

logger.LogInformation("Sending event...");
//TODO: Implement

await host.StopAsync();