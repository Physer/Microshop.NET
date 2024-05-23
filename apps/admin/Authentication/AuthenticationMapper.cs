using System.Text.Json;
using Application.Exceptions;
using Authentication.Models;

namespace Authentication;

internal static class AuthenticationMapper
{
    private static readonly JsonSerializerOptions _defaultJsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public static object MapToRequest(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new AuthenticationException();

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

    public static AuthenticationResponse MapFromResponse(string serializedResponse) => JsonSerializer.Deserialize<AuthenticationResponse>(serializedResponse, _defaultJsonSerializerOptions);
}
