﻿using System.Text.Json;
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

    public static AuthenticationResponse MapFromResponse(string serializedResponse, JsonSerializerOptions? serializerOptions = null)
    {
        if (string.IsNullOrWhiteSpace(serializedResponse))
            throw new ArgumentNullException(nameof(serializedResponse), "Invalid response data from Authentication service");

        serializerOptions ??= new JsonSerializerOptions(JsonSerializerDefaults.Web);
        return JsonSerializer.Deserialize<AuthenticationResponse>(serializedResponse, serializerOptions);
    }
}