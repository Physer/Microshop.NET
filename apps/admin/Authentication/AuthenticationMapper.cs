using System.Text.Json;
using Authentication.Models;

namespace Authentication;

internal static class AuthenticationMapper
{
    public static object MapToRequest(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new UnauthorizedAccessException();

        return new
        {
            FormFields = new object[]
            {
                new
                {
                    Id = "email",
                    Value = username
                },
                new
                {
                    Id = "password",
                    Value = password
                }
            }
        };
    }

    public static AuthenticationResponse MapFromResponse(string serializedResponse, JsonSerializerOptions? serializerOptions = null)
    {
        if (string.IsNullOrWhiteSpace(serializedResponse))
            throw new ArgumentNullException(nameof(serializedResponse), "Invalid response data from Authentication service");

        if (serializerOptions is null)
            serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        return JsonSerializer.Deserialize<AuthenticationResponse>(serializedResponse, serializerOptions);
    }
}
