using Domain;

namespace Messaging.Messages;

public sealed record PricesGenerated(IEnumerable<Price> prices);