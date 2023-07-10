# API Gateway is set-up using YARP

## Status

Accepted

## Context

To keep things easy and understandable for external applications accessing Microshop.NET, one clear URL for API access is required.

## Decision

To keep this unified entrypoint for external applications, an API Gateway is used.
This Gateway is implemented using [YARP](https://microsoft.github.io/reverse-proxy/index.html).

## Consequences

All external inbound traffic to Microshop.NET should go through the API Gateway.
This API Gateway should always use HTTPS/TLS and should have internal network access to the downstream services.