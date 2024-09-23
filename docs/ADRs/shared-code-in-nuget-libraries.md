# Shared code is exposed through libraries hosted as NuGet packages

## Status

Accepted

## Context

Some applications will have to do the same or similar things. For instance, both the Indexing service and the Admin UI need to set up several Docker containers as part of their integration tests.

In order to prevent code duplication, certain logic can be shared between applications. This improves maintainability of the code and ease of implementation for consumers of the shared code. 

## Decision

To facilitate code sharing, all shared code should be made into a library. This library will be published as an (open-source) NuGet package on the [NuGet.org package feed](https://www.nuget.org/).

## Consequences

* Duplicate code needs to be moved to a library
* A library needs to have the same test coverage as regular applications
* A library needs a README
* Consumers are in charge of the version of the library they're using