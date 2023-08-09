using Domain;

namespace Messaging.Messages;

public sealed record ProductsGenerated(IEnumerable<Product> Products);
