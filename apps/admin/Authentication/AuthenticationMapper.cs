using System.Text.Json;
using Application.Exceptions;
using Authentication.Models;

namespace Authentication;

internal static class AuthenticationMapper
{
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

    public static AuthenticationResponse MapFromResponse(string serializedResponse) => JsonSerializer.Deserialize<AuthenticationResponse>(serializedResponse, new JsonSerializerOptions(JsonSerializerDefaults.Web));
}
