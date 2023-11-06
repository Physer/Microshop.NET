# AngleSharp is used for writing end-to-end integration tests

## Status

Accepted

## Context

With Razor Pages, .NET automatically setups [CSRF protection](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-7.0&tabs=visual-studio#xsrfcsrf-and-razor-pages).

This, however, presents a problem for integration testing. We'll need to find a way to bypass or comply with the antiforgery mechanic.

## Decision

To prevent bypassing CSRF and potentially creating a weak spot in the application, we will be using [AngleSharp](https://anglesharp.github.io/) as a DOM parser.
This allows us to construct the page as-is, without compromising on security.

## Consequences

Using [AngleSharp](https://anglesharp.github.io/), we can completely manipulate the DOM and all Razor Pages through code, without needing to bypass or disable certain aspects of the application.