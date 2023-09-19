namespace DataGeneration.Options;

internal class DataGenerationOptions
{
    public const string ConfigurationEntry = "DataGeneration";

    public required string BaseUrl { get; init; }
}
