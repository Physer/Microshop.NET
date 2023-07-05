# The Servicebus Topology is deployed separately per service before the platform starts

## Status

Accepted

## Context

Due to the distributed nature of the platform, we cannot guarantee that the consumers will always start before the publishers.
They need to do so because the consumers are responsible for creating the queues and binding the exchanges to them.
If the publisher starts first, it broadcasts a message on the bus without a binding or a queue being present, resulting in an undelivered (and non-retried) message.

## Decision

To always guarantee the presence of all our queues, exchanges and their bindings, every service should deploy their topology without starting the actual service before everything else.

## Consequences

This means that every service should be able to deploy their topology quickly and separately, stateless and ephemeral.
Pipelines have to be adjusted to cater for topology-only deployments as well as regular publisher/consumer deployments.
For more information about this situation, see:
* [MassTransit's documentation](https://masstransit.io/documentation/configuration/topology/deploy)
* [Chris' answer on the issue](https://stackoverflow.com/a/70427605/1784012)