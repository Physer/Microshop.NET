using Application.UserManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using UserManagement.Options;

[assembly: InternalsVisibleTo("UnitTests")]
namespace UserManagement;

public static class DependencyRegistrator
{
    public static void RegisterUserManagementDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var userManagementOptionsSection = configuration.GetSection(UserManagementOptions.ConfigurationEntry);
        var userManagementOptions = userManagementOptionsSection.Get<UserManagementOptions>();
        if (userManagementOptions is null)
            throw new ArgumentNullException(nameof(userManagementOptions), "Invalid user management options");

        services.AddHttpClient<IUserClient, UserClient>(configuration => configuration.BaseAddress = new Uri(userManagementOptions.BaseUrl));
    }
}