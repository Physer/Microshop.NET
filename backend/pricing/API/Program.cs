using API;

var builder = WebApplication.CreateBuilder(args);
ServiceConfigurator.ConfigureServices(builder.Configuration, builder.Services);
var app = builder.Build();
app.Run();

public partial class Program { }