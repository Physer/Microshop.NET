using Application.Options;
using Messaging;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Options
var servicebusOptionsSection = builder.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
builder.Services.Configure<ServicebusOptions>(servicebusOptionsSection);

// Messaging
builder.Services.RegisterMessagingDependencies(servicebusOptions);

// Reverse proxy
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection(ProxyOptions.ConfigurationEntry));

var app = builder.Build();
app.MapPost("/products", Endpoints.GenerateProducts);
app.MapReverseProxy();

app.Run();

public partial class Program { }