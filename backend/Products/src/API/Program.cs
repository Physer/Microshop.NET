using Application;
using Application.Commands.GenerateProducts;
using Application.Queries.GetProduct;
using Application.Queries.GetProducts;
using MediatR;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyPointer>());
builder.Services.AddSingleton<IRepository, ProductRepository>();
var app = builder.Build();

app.MapGet("/products", async (IMediator mediator) => await mediator.Send(new GetProductsQuery()));
app.MapGet("/products/{id}", async (IMediator mediator, Guid id) => await mediator.Send(new GetProductQuery { ProductId = id }));
app.MapPost("/products", async (IMediator mediator) => await mediator.Send(new GenerateProductsCommand()));

app.Run();
