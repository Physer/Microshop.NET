using Application;
using Application.Queries.GetProduct;
using Application.Queries.GetProducts;
using MediatR;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyPointer>());
builder.Services.AddSingleton<IRepository, ProductRepository>();
var app = builder.Build();

app.MapGet("/products", async (IMediator mediatr) => await mediatr.Send(new GetProductsQuery()));
app.MapGet("/products/{id}", async (IMediator mediatr, Guid id) => await mediatr.Send(new GetProductQuery { ProductId = id }));

app.Run();
