using Microsoft.Extensions.Configuration;

namespace Application.Configuration;

// WebApplicationFactory does not support custom configuration prior to .NET 8
// https://github.com/dotnet/aspnetcore/issues/37680
public static class TestConfiguration
{
    // This async local is set in from tests and it flows to main
    internal static readonly AsyncLocal<Action<IConfigurationBuilder>?> _current = new();

    /// <summary>
    /// Adds the current test configuration to the application in the "right" place
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder</param>
    /// <returns>The modified <see cref="IConfigurationBuilder"/></returns>
    public static IConfigurationBuilder AddTestConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        if (_current.Value is { } configure)
        {
            configure(configurationBuilder);
        }

        return configurationBuilder;
    }

    /// <summary>
    /// Unit tests can use this to flow state to the main program and change configuration
    /// </summary>
    /// <param name="action"></param>
    public static void Create(Action<IConfigurationBuilder> action)
    {
        _current.Value = action;
    }
}
