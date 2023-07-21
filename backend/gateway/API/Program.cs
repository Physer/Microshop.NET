using Application.Options;
using Messaging;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Options
var servicebusOptionsSection = builder.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
builder.Services.Configure<ServicebusOptions>(servicebusOptionsSection);

// Messaging
builder.Services.RegisterMessagingDependencies(servicebusOptions); var app = builder.Build();

app.MapPost("/products", Endpoints.GenerateProducts);

app.Run();

public partial class Program { }