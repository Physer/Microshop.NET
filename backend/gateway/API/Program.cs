using API;
using API.Authentication;
using Application.Options;
using Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Options
var servicebusOptionsSection = builder.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
builder.Services.Configure<ServicebusOptions>(servicebusOptionsSection);
builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.ConfigurationEntry));

// Messaging
builder.Services.RegisterMessagingDependencies(servicebusOptions);

// Authentication and authorization
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicroshopJwtBearer();
builder.Services.AddAuthorization();

// Reverse proxy
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection(ProxyOptions.ConfigurationEntry));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapPost("/products", Endpoints.GenerateProducts);
app.MapReverseProxy();

app.Run();

public partial class Program { }