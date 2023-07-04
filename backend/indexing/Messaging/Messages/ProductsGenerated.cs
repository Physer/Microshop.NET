using Domain;

namespace Messaging.Messages;

public record ProductsGenerated(IEnumerable<Product> Products);
