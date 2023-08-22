# Supertokens for authentication and authorization

## Status

Accepted

## Context

Authentication and authorization are required components of any web application with users.

This means that for Microshop.NET, these two components are also required. In order to prevent writing a custom authentication provider and implementation,
an existing solution was chosen.

## Decision

The authentication and authorization solution that was chosen is [SuperTokens](https://supertokens.com/). An open-source authentication framework.

## Consequences

- All authentication should be going through SuperTokens
- SuperTokens has no SDK for .NET
- SuperTokens preferred SDK for Microshop.NET is the Go SDK
- SuperTokens' dashboard is used for user management