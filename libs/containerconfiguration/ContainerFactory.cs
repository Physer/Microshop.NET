using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microshop.ContainerConfiguration.ContainerConfigurations;
using Microshop.ContainerConfiguration.Exceptions;

namespace Microshop.ContainerConfiguration;

public static class ContainerFactory
{
    public static ContainerConfigurationResponse<ServicebusContainerConfiguration>? ServicebusContainerConfiguration { get; private set; }
    public static ContainerConfigurationResponse<IndexContainerConfiguration>? IndexContainerConfiguration { get; private set; }
    public static ContainerConfigurationResponse<PostgresContainerConfiguration>? PostgresContainerConfiguration { get; private set; }
    public static ContainerConfigurationResponse<SupertokensContainerConfiguration>? SupertokensContainerConfiguration { get; private set; }
    public static ContainerConfigurationResponse<AuthenticationServiceConfiguration>? AuthenticationServiceContainerConfiguration { get; private set; }
    public static ContainerConfigurationResponse<MicroshopApiConfiguration>? MicroshopApiContainerConfiguration { get; private set; }


    /// <summary>
    /// Initialize all the predefined containers.
    /// Note that this method is required in order to access the container properties.
    /// </summary>
    /// <exception cref="ContainerNotInitializedException">If the properties of a container is null after initialization, this is thrown</exception>
    public static async Task InitializeAllPredefinedContainers()
    {
        await InitializeServicebusContainerAsync();
        if (ServicebusContainerConfiguration is null)
            throw new ContainerNotInitializedException(nameof(ServicebusContainerConfiguration));

        await InitializeIndexContainerAsync();
        if (IndexContainerConfiguration is null)
            throw new ContainerNotInitializedException(nameof(IndexContainerConfiguration));

        await InitializePostgresContainerAsync();
        if (PostgresContainerConfiguration is null)
            throw new ContainerNotInitializedException(nameof(PostgresContainerConfiguration));

        await InitializeSupertokensContainerAsync(PostgresContainerConfiguration.Configuration.InternalConnectionString);
        if (SupertokensContainerConfiguration is null)
            throw new ContainerNotInitializedException(nameof(SupertokensContainerConfiguration));

        await InitializeAuthenticationServiceContainerAsync(SupertokensContainerConfiguration.Container.IpAddress, SupertokensContainerConfiguration.Configuration.Port!.Value);
        if (AuthenticationServiceContainerConfiguration is null)
            throw new ContainerNotInitializedException(nameof(AuthenticationServiceContainerConfiguration));

        await InitializeMicroshopApiContainerAsync(AuthenticationServiceContainerConfiguration.Container.IpAddress, ServicebusContainerConfiguration.Container.IpAddress, ServicebusContainerConfiguration.Configuration.Username, ServicebusContainerConfiguration.Configuration.Password);
        if (MicroshopApiContainerConfiguration is null)
            throw new ContainerNotInitializedException(nameof(MicroshopApiContainerConfiguration));
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

    private static async Task InitializeServicebusContainerAsync()
    {
        var servicebusConfiguration = new ServicebusContainerConfiguration();
        var container = await InitializePredefinedContainerAsync(servicebusConfiguration);
        ServicebusContainerConfiguration = new(container, servicebusConfiguration);
    }

    private static async Task InitializeIndexContainerAsync()
    {
        var indexConfiguration = new IndexContainerConfiguration();
        var container = await InitializePredefinedContainerAsync(indexConfiguration);
        IndexContainerConfiguration = new(container, indexConfiguration);
    }

    private static async Task InitializePostgresContainerAsync()
    {
        var postgresConfiguration = new PostgresContainerConfiguration();
        var container = await InitializePredefinedContainerAsync(postgresConfiguration);
        postgresConfiguration.ContainerIpAddress = container.IpAddress;
        postgresConfiguration.PublicPort = container.GetMappedPublicPort(postgresConfiguration.Port!.Value);
        PostgresContainerConfiguration = new(container, postgresConfiguration);
    }

    private static async Task InitializeSupertokensContainerAsync(string supertokensDatabaseConnectionString)
    {
        var supertokensConfiguration = new SupertokensContainerConfiguration
        {
            AuthenticationDatabaseConnectionString = supertokensDatabaseConnectionString
        };
        var container = await InitializePredefinedContainerAsync(supertokensConfiguration);
    }

    private static async Task InitializeAuthenticationServiceContainerAsync(string supertokensContainerIpAddress, int supertokensContainerPort)
    {
        var authenticationServiceContainer = new AuthenticationServiceConfiguration
        {
            SupertokensContainerIpAddress = supertokensContainerIpAddress,
            SupertokensContainerPort = supertokensContainerPort
        };
        var container = await InitializePredefinedContainerAsync(authenticationServiceContainer);
        AuthenticationServiceContainerConfiguration = new(container, authenticationServiceContainer);
    }

    private static async Task InitializeMicroshopApiContainerAsync(string authenticationServiceContainerIp, string servicebusContainerIp, string servicebusUsername, string servicebusPassword)
    {
        var microshopApiContainer = new MicroshopApiConfiguration
        {
            AuthenticationServiceContainerIp = authenticationServiceContainerIp,
            RabbitMqContainerIp = servicebusContainerIp,
            RabbitMqPassword = servicebusUsername,
            RabbitMqUsername = servicebusPassword
        };
        var container = await InitializePredefinedContainerAsync(microshopApiContainer);
        MicroshopApiContainerConfiguration = new(container, microshopApiContainer);
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
