using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IntegrationTests;

internal class IntegrationAuthenticationHandler : AuthenticationHandler<IntegrationAuthenticationOptions>
{
    public const string IntegrationTestSchema = "Integration";

    public IntegrationAuthenticationHandler(IOptionsMonitor<IntegrationAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(IntegrationTestSchema);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, IntegrationTestSchema);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
