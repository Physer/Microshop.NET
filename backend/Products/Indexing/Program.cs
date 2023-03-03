using Indexing.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Options
builder.Services.Configure<IndexingOptions>(configuration.GetSection(IndexingOptions.ConfigurationEntry));
builder.Services.Configure<ProductsOptions>(configuration.GetSection(ProductsOptions.ConfigurationEntry));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
