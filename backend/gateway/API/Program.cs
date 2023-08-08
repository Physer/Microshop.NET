using Application.Options;
using Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Options
var servicebusOptionsSection = builder.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
builder.Services.Configure<ServicebusOptions>(servicebusOptionsSection);

var authenticationOptions = builder.Configuration.GetSection(AuthenticationOptions.ConfigurationEntry).Get<AuthenticationOptions>();
if (authenticationOptions is null)
    throw new ArgumentNullException(nameof(authenticationOptions), "Invalid authentication options");

// Messaging
builder.Services.RegisterMessagingDependencies(servicebusOptions);

// Authentication and authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, async c =>
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri(authenticationOptions.BaseUrl);
    var jwksResponseMessage = await client.GetAsync(authenticationOptions.RelativeJwksEndpoint);
    var jwksJson = await jwksResponseMessage.Content.ReadAsStringAsync();
    var jwks = new JsonWebKeySet(jwksJson) ?? throw new UnauthorizedAccessException("Unable to validate the JWKS");

    c.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKeys = jwks.Keys,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});
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