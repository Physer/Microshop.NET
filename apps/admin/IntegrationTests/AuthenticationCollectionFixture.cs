using Xunit;

namespace IntegrationTests;

[CollectionDefinition(nameof(AuthenticationCollectionFixture))]
public class AuthenticationCollectionFixture : ICollectionFixture<AuthenticationFixture> { }
