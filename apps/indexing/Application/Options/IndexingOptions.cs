﻿namespace Application.Options;

public class IndexingOptions
{
    public const string ConfigurationEntry = "Indexing";

    public string? BaseUrl { get; set; }
    public string? ApiKey { get; set; }
    public int? IndexingIntervalInSeconds { get; set; }
}
