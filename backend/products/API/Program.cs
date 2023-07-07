using API;
using Application.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddTestConfiguration();
ServiceConfigurator.ConfigureServices(builder.Configuration, builder.Services);
var app = builder.Build();

app.MapPost("/products", Endpoints.GenerateProducts);

app.Run();

public partial class Program { }