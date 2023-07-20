using Application.Options;
using Generator;
using Messaging;

var builder = WebApplication.CreateBuilder(args);

// Options
var servicebusOptionsSection = builder.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>() ?? throw new NullReferenceException("Invalid servicebus options");
builder.Services.Configure<ServicebusOptions>(servicebusOptionsSection);

// Generator
builder.Services.RegisterGeneratorDependencies();

// Messaging
builder.Services.RegisterMessagingDependencies(servicebusOptions);

var app = builder.Build();

app.Run();
