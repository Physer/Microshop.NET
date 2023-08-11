using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Encodings.Web;
using AuthenticationOptions = Application.Options.AuthenticationOptions;

namespace API.Authentication;

internal class MicroshopAuthenticationHandler : JwtBearerHandler
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly ILogger<MicroshopAuthenticationHandler> _logger;

    public MicroshopAuthenticationHandler(IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IHttpClientFactory httpClientFactory,
        IOptions<AuthenticationOptions> authenticationOptions) : base(options, logger, encoder, clock)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(authenticationOptions.Value.BaseUrl);
        _authenticationOptions = authenticationOptions.Value;
        _logger = logger.CreateLogger<MicroshopAuthenticationHandler>();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            var jwksResponseMessage = await _httpClient.GetAsync(_authenticationOptions.RelativeJwksEndpoint);
            var jwksJson = await jwksResponseMessage.Content.ReadAsStringAsync();
            var jwks = new JsonWebKeySet(jwksJson);
            Options.TokenValidationParameters = new()
            {
                IssuerSigningKeys = jwks.Keys,
                ValidIssuer = _authenticationOptions.Issuer,
                ValidateAudience = false
            };

            return await base.HandleAuthenticateAsync();
        }
        catch (Exception ex)
        {
            var errorMessage = "Unable to retrieve the JWKS from the Authentication service";
            _logger.LogError(ex, "{errorMessage}", errorMessage);
            return AuthenticateResult.Fail(errorMessage);
        }
    }
}
