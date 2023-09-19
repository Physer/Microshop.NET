﻿using Application.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Options;

namespace UserManagement;

public static class DependencyRegistrator
{
    public static void RegisterUserManagementDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var userManagementOptionsSection = configuration.GetSection(UserManagementOptions.ConfigurationEntry);
        var userManagementOptions = userManagementOptionsSection.Get<UserManagementOptions>();
        if (userManagementOptions is null)
            throw new ArgumentNullException(nameof(userManagementOptions), "Invalid authentication options");

        services.AddHttpClient<IUserClient, UserClient>(configuration => configuration.BaseAddress = new Uri(userManagementOptions.BaseUrl));
    }
}