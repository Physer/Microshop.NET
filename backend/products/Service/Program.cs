using Application.Options;
using Generator;
using Messaging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Options
        var servicebusOptionsSection = context.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servicebusOptionsSection);

        // Generator
        services.RegisterGeneratorDependencies();

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    })
    .Build();

host.Run();
