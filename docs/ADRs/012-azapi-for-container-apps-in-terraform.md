# AzApi Terraform provider is used instead of AzureRM for deploying Azure Container Apps

## Status

Accepted

## Context

At least one of our Azure Container Apps requires to communicate through pure TCP rather than HTTP.
This setting is relatively new on Azure Container Apps.
The AzureRM Terraform provider by Hashicorp is not supporting this new transport mode yet.

## Decision

In order to support the `tcp` transport mode on Azure Container Apps deployments through infrastructure-as-code, [the AzApi provider by Azure](https://registry.terraform.io/providers/Azure/azapi/latest/docs) will be used in Terraform.

## Consequences

When deploying Azure Container Apps using Terraform, use the AzApi provider and its options.
For all other resources, the AzureRM provider can be used.
