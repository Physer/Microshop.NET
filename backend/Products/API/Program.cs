using API;
using Application;
using Generator;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyPointer>());
builder.Services.AddSingleton<IRepository, ProductRepository>();
builder.Services.AddTransient<IProductGenerator, ProductGenerator>();
var app = builder.Build();

app.MapGet("/products", ProductEndpoints.GetAllProducts);
app.MapGet("/products/{id}", ProductEndpoints.GetProduct);
app.MapPost("/products", ProductEndpoints.GenerateProducts);

app.Run();
