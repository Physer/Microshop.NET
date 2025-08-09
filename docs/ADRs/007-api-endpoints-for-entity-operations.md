# API endpoints are created for entity operations

## Status

Accepted

## Context

Whenever an entity's state is changed (e.g. products are created) an event should be sent out so other services can act on this.
However, if this is done on startup of a service, there's no guarantee other services will be listening.
It also doesn't allow for any fine-grained control or retrying of sending messages.

## Decision

To have control over when to send which messages, API endpoints will be created for all entity management operations (e.g. product creation).

## Consequences

For every application that requires interaction with data other than simply listening to events, an API is required.
