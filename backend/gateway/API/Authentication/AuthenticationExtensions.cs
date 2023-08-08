using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace API.Authentication;

public static class AuthenticationExtensions
{
    public static AuthenticationBuilder AddMicroshopJwtBearer(this AuthenticationBuilder authenticationBuilder)
    {
        authenticationBuilder.AddScheme<JwtBearerOptions, MicroshopAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, _ => { });

        return authenticationBuilder;
    }
}
