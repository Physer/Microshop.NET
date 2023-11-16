using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Microshop.ContainerConfiguration;

public static class ContainerFactory
{
    /// <summary>
    /// Initialize the servicebus container powered by MassTransit and RabbitMQ
    /// </summary>
    public static async Task<ContainerConfigurationResponse<ServicebusContainerConfiguration>> InitializeServicebusContainerAsync()
    {
        var servicebusConfiguration = new ServicebusContainerConfiguration();
        var container = await InitializePredefinedContainerAsync(servicebusConfiguration);
        return new(container, servicebusConfiguration);
    }

    /// <summary>
    /// Initialize the search index container powered by Meilisearch
    /// </summary>
    public static async Task<ContainerConfigurationResponse<IndexContainerConfiguration>> InitializeIndexContainerAsync()
    {
        var indexConfiguration = new IndexContainerConfiguration();
        var container = await InitializePredefinedContainerAsync(indexConfiguration);
        return new(container, indexConfiguration);
    }

    /// <summary>
    /// Initialize authentication database container powered by Postgres
    /// </summary>
    public static async Task<ContainerConfigurationResponse<PostgresContainerConfiguration>> InitializePostgresContainerAsync()
    {
        var postgresConfiguration = new PostgresContainerConfiguration();
        var container = await InitializePredefinedContainerAsync(postgresConfiguration);
        return new(container, postgresConfiguration);
    }

    /// <summary>
    /// Initialize the authentication core container powered by Supertokens
    /// </summary>
    /// <param name="supertokensDatabaseConnectionString">The Postgres ConnectionString to use for the authentication database</param>
    public static async Task<ContainerConfigurationResponse<SupertokensContainerConfiguration>> InitializeSupertokensContainerAsync(string supertokensDatabaseConnectionString)
    {
        var supertokensConfiguration = new SupertokensContainerConfiguration
        {
            AuthenticationDatabaseConnectionString = supertokensDatabaseConnectionString
        };
        var container = await InitializePredefinedContainerAsync(supertokensConfiguration);
        return new(container, supertokensConfiguration);
    }

    /// <summary>
    /// Initialize the authentication backend container powered by Microshop
    /// </summary>
    /// <param name="supertokensContainerIpAddress">The IP address of the container serving the authentication core</param>
    /// <param name="supertokensContainerPort">The port of the container serving the authentication core</param>
    public static async Task<ContainerConfigurationResponse<AuthenticationServiceConfiguration>> InitializeAuthenticationServiceContainerAsync(string supertokensContainerIpAddress, int supertokensContainerPort)
    {
        var authenticationServiceContainer = new AuthenticationServiceConfiguration
        {
            SupertokensContainerIpAddress = supertokensContainerIpAddress,
            SupertokensContainerPort = supertokensContainerPort
        };
        var container = await InitializePredefinedContainerAsync(authenticationServiceContainer);
        return new(container, authenticationServiceContainer);
    }

    /// <summary>
    /// Initialize the API container powered by Microshop
    /// </summary>
    /// <param name="authenticationServiceContainerIp">The IP address of the container serving the authentication service</param>
    /// <param name="servicebusContainerIp">The IP address of the container serving the servicebus</param>
    /// <param name="servicebusUsername">The management username of the servicebus</param>
    /// <param name="servicebusPassword">The management passowrd of the servicebus</param>
    public static async Task<ContainerConfigurationResponse<MicroshopApiConfiguration>> InitializeMicroshopApiContainerAsync(string authenticationServiceContainerIp, string servicebusContainerIp, string servicebusUsername, string servicebusPassword)
    {
        var microshopApiContainer = new MicroshopApiConfiguration
        {
            AuthenticationServiceContainerIp = authenticationServiceContainerIp,
            RabbitMqContainerIp = servicebusContainerIp,
            RabbitMqPassword = servicebusUsername,
            RabbitMqUsername = servicebusPassword
        };
        var container = await InitializePredefinedContainerAsync(microshopApiContainer);
        return new(container, microshopApiContainer);
    }

    /// <summary>
    /// Initialize a custom container based on an implementation of the IContainerConfiguration interface
    /// </summary>
    /// <param name="customContainerConfiguration">The configuration of the custom container</param>
    public static async Task<ContainerConfigurationResponse<IContainerConfiguration>> InitializeCustomContainerAsync(IContainerConfiguration customContainerConfiguration)
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
        return new(container, customContainerConfiguration);
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
