using Application.Interfaces.Generator;
using Application.Interfaces.Repositories;
using Application.Options;
using Consumer;
using Generator;
using Mapper;
using Persistence;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Options
        var productOptionsSection = context.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var productOptions = productOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(productOptionsSection);

        // Persistence
        services.AddSingleton<IRepository, ProductRepository>();

        // Generator
        services.AddTransient<IProductGenerator, ProductGenerator>();

        // Mapper
        services.AddAutoMapper(typeof(ProductProfile));

        // Consumer
        services.RegisterConsumerDependencies(productOptions);
    })
    .Build();

GenerateProductData(host);
host.Run();

// Generating some dummy product data on startup
static void GenerateProductData(IHost host)
{
    var amountOfProductsToGenerate = 500;
    Console.WriteLine($"Generating {amountOfProductsToGenerate} fake products...");
    var productGenerator = host.Services.GetRequiredService<IProductGenerator>();
    productGenerator.GenerateProducts(amountOfProductsToGenerate);
    Console.WriteLine("Succesfully generated product data!");
}