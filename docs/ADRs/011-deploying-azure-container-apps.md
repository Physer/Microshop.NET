# Azure Container Apps are used as lightweight Kubernetes alternative

## Status

Accepted

## Context

Kubernetes clusters are expensive to run and maintain.
It also requires additional knowledge of components such as KEDA and Envoy.
Azure offers a lightweight, managed alternative for Kubernetes orchestration without having to worry about the underlying infrastructure.

## Decision

To minimize operating costs and complexity, Azure Container Apps will be used for deployments rather than Kubernetes.

## Consequences

For local development, `docker-compose` can be used as a lightweight orchestrator.
For Cloud deployments, Azure Container Apps will be used.
Terraform remains the solution for infrastructure-as-code.
