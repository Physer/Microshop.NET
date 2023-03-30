# Kubernetes for cloud resources

## Status

Accepted

## Context

We want to stay as cloud-agnostic as possible so we don't lock ourselves to a certain cloud provider.
This means that all our resources should run on any cloud and we should refrain from using cloud specific functionalities (e.g. Azure Function, AWS Lambda etc.)

## Decision

In order to achieve this agnostic state, we will run everything on Kubernetes through our own applications.

## Consequences

We have no, or limited, vendor lock-in when it comes to cloud resources and our code uses the same tooling across every environment, including local development.