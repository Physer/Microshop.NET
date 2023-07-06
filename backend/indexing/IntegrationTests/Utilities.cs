namespace IntegrationTests;

internal static class Utilities
{
    public static string[] CreateTemporaryCommandlineArguments(IDictionary<string, string?>? configuration = default)
    {
        var arguments = new List<string>() { "ShouldStop" };

        if (configuration?.Any() == true)
        {
            var configurationAsCommandlineArguments = ConvertConfigurationToCommandLineArguments(configuration);
            arguments = arguments.Concat(configurationAsCommandlineArguments).ToList();
        }

        return arguments.ToArray();
    }

    private static IEnumerable<string> ConvertConfigurationToCommandLineArguments(IDictionary<string, string?> configuration) => configuration.Select(c => $"--{c.Key}={c.Value}");
}
