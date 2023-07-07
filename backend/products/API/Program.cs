using API;

var builder = WebApplication.CreateBuilder(args);
ServiceConfigurator.ConfigureServices(builder.Configuration, builder.Services);
var app = builder.Build();

app.MapPost("/products", Endpoints.GenerateProducts);

app.Run();

public partial class Program { }