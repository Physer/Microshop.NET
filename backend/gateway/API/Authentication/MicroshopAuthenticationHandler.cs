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
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var jwksResponseMessage = await _httpClient.GetAsync(_authenticationOptions.RelativeJwksEndpoint);
        var jwksJson = await jwksResponseMessage.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(jwksJson))
            return AuthenticateResult.Fail("Unable to retrieve the JSON Web Key Set from the Authentication service");

        var jwks = new JsonWebKeySet(jwksJson);
        Options.TokenValidationParameters = new()
        {
            IssuerSigningKeys = jwks.Keys,
            ValidIssuer = _authenticationOptions.Issuer,
            ValidateAudience = false
        };

        return await base.HandleAuthenticateAsync();
    }
}
