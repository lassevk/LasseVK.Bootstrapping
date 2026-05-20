# LasseVK.Bootstrapping

[![build](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/build.yml/badge.svg)](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/build.yml)
[![codeql](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/github-code-scanning/codeql)

This package implements a way to handle registration of services and configuration in
a .NET application using the [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting)
package.

It allows you to place a `ModuleBootstrapper` class in each class library, and invoke those during program setup
to allow them to register services, without having to expose internal implementations from the libraries.

This means you can move services and configuration registration out of `Program.cs` and into the libraries where
the functionality and services reside.

## Discord

You can reach me through my [Discord server](https://discord.gg/xz2N2XZCV5).

## Nuget package

You can find the Nuget package [here](https://www.nuget.org/packages/LasseVK.Bootstrapping/).

## Installation

You can install the package from the command line, using `dotnet`:

```bash
dotnet add package LasseVK.Bootstrapping
```

Or you can use your favorite IDE which should have a Nuget package manager built in.

## Framework support

The package supports the following .NET versions:

* .NET 8.0 (until november 10, 2026)
* .NET 9.0 (until november 10, 2026)
* .NET 10.0 (until november 14, 2028)

This follows the official supported versions policies from Microsoft:

* [The official .NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy)

*Note:* After support for a .NET version ends, the package will still exist on nuget for use with
that version, but I won't guarantee that updates to that version will be made.

# Usage

TODO
