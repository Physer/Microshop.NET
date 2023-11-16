using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Microshop.ContainerConfiguration;

public static class ContainerFactory
{
    /// <summary>
    /// Initialize the servicebus container powered by MassTransit and RabbitMQ
    /// </summary>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitializeServicebusContainerAsync()
    {
        var rabbitMqConfiguration = new ServicebusContainerConfiguration();
        return await InitializePredefinedContainerAsync(rabbitMqConfiguration);
    }

    /// <summary>
    /// Initialize the search index container powered by Meilisearch
    /// </summary>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitializeIndexContainerAsync()
    {
        var meilisearchConfiguration = new IndexContainerConfiguration();
        return await InitializePredefinedContainerAsync(meilisearchConfiguration);
    }

    /// <summary>
    /// Initialize authentication database container powered by Postgres
    /// </summary>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitliazePostgresContainerAsync()
    {
        var postgresConfiguration = new PostgresContainerConfiguration();
        return await InitializePredefinedContainerAsync(postgresConfiguration);
    }

    /// <summary>
    /// Initialize the authentication core container powered by Supertokens
    /// </summary>
    /// <param name="supertokensDatabaseConnectionString">The Postgres ConnectionString to use for the authentication database</param>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitializeSupertokensContainerAsync(string supertokensDatabaseConnectionString)
    {
        var supertokensConfiguration = new SupertokensContainerConfiguration
        {
            AuthenticationDatabaseConnectionString = supertokensDatabaseConnectionString
        };
        return await InitializePredefinedContainerAsync(supertokensConfiguration);
    }

    /// <summary>
    /// Initialize the authentication backend container powered by Microshop
    /// </summary>
    /// <param name="supertokensContainerIpAddress">The IP address of the container serving the authentication core</param>
    /// <param name="supertokensContainerPort">The port of the container serving the authentication core</param>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitializeAuthenticationServiceContainerAsync(string supertokensContainerIpAddress, int supertokensContainerPort)
    {
        var authenticationServiceContainer = new AuthenticationServiceConfiguration
        {
            SupertokensContainerIpAddress = supertokensContainerIpAddress,
            SupertokensContainerPort = supertokensContainerPort
        };
        return await InitializePredefinedContainerAsync(authenticationServiceContainer);
    }

    /// <summary>
    /// Initialize the API container powered by Microshop
    /// </summary>
    /// <param name="authenticationServiceContainerIp">The IP address of the container serving the authentication service</param>
    /// <param name="servicebusContainerIp">The IP address of the container serving the servicebus</param>
    /// <param name="servicebusUsername">The management username of the servicebus</param>
    /// <param name="servicebusPassword">The management passowrd of the servicebus</param>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitializeMicroshopApiContainerAsync(string authenticationServiceContainerIp, string servicebusContainerIp, string servicebusUsername, string servicebusPassword)
    {
        var microshopApiContainer = new MicroshopApiConfiguration
        {
            AuthenticationServiceContainerIp = authenticationServiceContainerIp,
            RabbitMqContainerIp = servicebusContainerIp,
            RabbitMqPassword = servicebusUsername,
            RabbitMqUsername = servicebusPassword
        };
        return await InitializePredefinedContainerAsync(microshopApiContainer);
    }

    /// <summary>
    /// Initialize a custom container based on an implementation of the IContainerConfiguration interface
    /// </summary>
    /// <param name="customContainerConfiguration">The configuration of the custom container</param>
    /// <returns>The container instance</returns>
    public static async Task<IContainer> InitializeCustomContainerAsync(IContainerConfiguration customContainerConfiguration)
    {
        var containerBuilder = new ContainerBuilder()
            .WithImage(customContainerConfiguration.ImageName)
            .WithEnvironment(customContainerConfiguration.EnvironmentVariables);

        if (customContainerConfiguration.Port.HasValue)
        {
            containerBuilder = containerBuilder.WithPortBinding(customContainerConfiguration.Port.Value, true);
            containerBuilder = containerBuilder.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(customContainerConfiguration.Port.Value));
        }
        else
            containerBuilder = containerBuilder.WithWaitStrategy(Wait.ForUnixContainer());

        var container = containerBuilder.Build();
        await container.StartAsync().ConfigureAwait(false);
        return container;
    }

    private static async Task<IContainer> InitializePredefinedContainerAsync(IContainerConfiguration containerConfiguration)
    {
        var container = new ContainerBuilder()
            .WithImage(containerConfiguration.ImageName)
            .WithEnvironment(containerConfiguration.EnvironmentVariables)
            .WithPortBinding(containerConfiguration.Port!.Value, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(containerConfiguration.Port!.Value))
            .Build();
        await container.StartAsync().ConfigureAwait(false);
        return container;
    }
}
