using Indexer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<IndexService>();
    })
    .Build();

host.Run();
