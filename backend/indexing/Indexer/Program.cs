using Indexer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ServiceConfigurator.ConfigureServices)
    .Build();

host.Run();
