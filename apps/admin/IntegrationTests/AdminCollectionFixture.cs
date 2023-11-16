using Xunit;

namespace IntegrationTests;

[CollectionDefinition(nameof(AdminCollectionFixture))]
public class AdminCollectionFixture : ICollectionFixture<AdminFixture> { }
