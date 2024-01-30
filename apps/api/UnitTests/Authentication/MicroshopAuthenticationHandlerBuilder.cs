using API.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Net;
using System.Text.Encodings.Web;
using AuthenticationOptions = Application.Options.AuthenticationOptions;

namespace UnitTests.Authentication;

internal class MicroshopAuthenticationHandlerBuilder
{
    private readonly IOptionsMonitor<JwtBearerOptions> _options;
    private readonly ILoggerFactory _logger;
    private readonly UrlEncoder _encoder;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<AuthenticationOptions> _authenticationOptions;
    
    public MicroshopAuthenticationHandlerBuilder()
    {
        _options = Substitute.For<IOptionsMonitor<JwtBearerOptions>>();
        _options.Get(Arg.Any<string>()).Returns(new JwtBearerOptions());

        _logger = Substitute.For<ILoggerFactory>();
        _encoder = Substitute.For<UrlEncoder>();
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _authenticationOptions = Substitute.For<IOptions<AuthenticationOptions>>();
    }

    public MicroshopAuthenticationHandlerBuilder WithAuthenticationOptions(AuthenticationOptions options)
    {
        _authenticationOptions.Value.Returns(options);

        return this;
    }

    public MicroshopAuthenticationHandlerBuilder WithJwksEndpointReturningData(object? response)
    {
        _httpClientFactory.CreateClient().Returns(new HttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, response)));

        return this;
    }

    public MicroshopAuthenticationHandler Build() => new(_options, _logger, _encoder, _httpClientFactory, _authenticationOptions);
}
