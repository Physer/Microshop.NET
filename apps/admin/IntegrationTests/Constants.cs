using System.Text.Json;

namespace IntegrationTests;

internal static class Constants
{
    public const string DefaultTextValue = "integration_tests";
    public const string DefaultPasswordValue = "P@ssw0rd!";

    public static JsonSerializerOptions DefaultJsonSerializerOptions = new(JsonSerializerDefaults.Web);
}
