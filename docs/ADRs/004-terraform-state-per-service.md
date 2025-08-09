# Every service has their own Terraform state

## Status

Accepted

## Context

In order to keep services isolated and deployable separately, a monolithic Terraform plan should be prevented.

## Decision

Every service (or related services) should have their own Terraform state and their own deployable.

## Consequences

- Services should be able to access other states by using the `terraform_remote_state` data source
- Terraform deployments should not depend on other deployments (in other workflows)
