# AMQP for inter-services communication

## Status

Accepted

## Context

Since services have to talk to each other, we want to do so in an asynchronous, non-blocking way.
There should be no dependencies between services.

## Decision

All communication between services, whether it's querying data or applying mutations, should go through the AMQ Protocol.
This also means that HTTP or REST cannot be used for communication between services.

## Consequences

Advantages to choose AMQP over HTTP:

* Asynchronous processing (not waiting for a response)
* Lower latency
* Better performance
* Better separation of concerns
* No knowledge of data context of other services
