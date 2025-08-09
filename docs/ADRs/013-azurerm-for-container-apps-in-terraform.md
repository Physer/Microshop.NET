# AzureRM Terraform provider is used (the default) for deploying Azure Container Apps

## Status

Accepted

## Context

In the ADR: [012-azapi-for-container-apps-in-terraform](012-azapi-for-container-apps-in-terraform.md), it was decided to use the `AzApi` Terraform provider due to limitations on the regular `AzureRM` Terraform provider.
Since the `AzureRM` has been updated, these limitations no longer apply.

## Decision

As the default Azure terraform provider, `AzureRM` shall be used for all Azure related resources in Terraform. The `AzApi` provider is no longer required.

## Consequences

The `AzureRM` Terraform provider will be used for all Azure resources unless specifically stated in a separate ADR.
