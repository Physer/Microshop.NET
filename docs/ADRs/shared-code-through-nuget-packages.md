# Shared code is used through NuGet packages

## Status

Accepted

## Context

Applications will have overlapping code, causing potential code duplication.
To prevent this duplication, code can be shared among applications.

However, code shouldn't be shared directly as this compromises isolation and maintainability.
Whenever a common piece of functionality needs to be reused, a library should be developed.

## Decision

Every reusable piece of code across different applications should be extracted to its own library.
This library will then be uploaded as a NuGet package that can be consumed by the applications.
The applications themselves will then be responsible for keeping up to date.

## Consequences

To share code in a reliable way without compromising isolation of microservices.
