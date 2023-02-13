using Application;
using Application.Queries;
using MediatR;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(typeof(ApplicationAssemblyPointer));
builder.Services.AddSingleton<IRepository, ProductRepository>();
var app = builder.Build();

app.MapGet("/products", async (IMediator mediatr) => await mediatr.Send(new GetProductsQuery()));

app.Run();
