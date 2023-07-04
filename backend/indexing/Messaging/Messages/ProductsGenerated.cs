using Domain;

namespace Messaging.Messages;

internal record ProductsGenerated(IEnumerable<Product> Products);
