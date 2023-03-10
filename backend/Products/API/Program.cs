using API;
using Application;
using Application.Interfaces.Generator;
using Application.Interfaces.Repositories;
using Application.Options;
using Consumer;
using Generator;
using Mapper;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Options
var productOptionsSection = builder.Configuration.GetSection(ProductsOptions.ConfigurationEntry);
var productOptions = productOptionsSection.Get<ProductsOptions>();
builder.Services.Configure<ProductsOptions>(productOptionsSection);

// MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyPointer>());

// Persistence
builder.Services.AddSingleton<IRepository, ProductRepository>();

// Generator
builder.Services.AddTransient<IProductGenerator, ProductGenerator>();

// Mapper
builder.Services.AddAutoMapper(typeof(ProductProfile));

// Consumer
builder.Services.RegisterConsumerDependencies(productOptions);

var app = builder.Build();

app.MapGet("/products", ProductEndpoints.GetAllProducts);
app.MapGet("/products/{id}", ProductEndpoints.GetProduct);
app.MapPost("/products", ProductEndpoints.GenerateProducts);

app.Run();
