global using FastEndpoints;

using Application;
using Application.Queries.GetProduct;
using MediatR;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyPointer>());
builder.Services.AddSingleton<IRepository, ProductRepository>();
builder.Services.AddFastEndpoints();
var app = builder.Build();

app.UseAuthorization();
app.UseFastEndpoints();

app.MapGet("/products/{id}", async (IMediator mediatr, Guid id) => await mediatr.Send(new GetProductQuery { ProductId = id }));

app.Run();
