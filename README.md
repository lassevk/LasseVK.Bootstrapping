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

After installing the package in your projects, you can create a `ModuleBootstrapper` class in each class library, and invoke those during program setup
to allow them to register services, without having to expose internal implementations from the libraries.

This means you can move services and configuration registration out of `Program.cs` and into the libraries where
the functionality and services reside.

Here's an example, in a separate class library you can declare the following two types:

```csharp
public interface ISomeService
{
    string GetSomeValue();
}

internal class SomeService : ISomeService
{
    public string GetSomeValue()
    {
        return "Some value";
    }
}
```

then you also declare a `ModuleBootstrapper` class in the same project:

```csharp
public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISomeService, SomeService>();
    }
}
```

I also tend to add an `ApplicationBootstrapper` class in my main program project that registers
everything that is specific to the program:

```csharp
public class ApplicationBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new TheClassLibrary.ModuleBootstrapper());
        
        // specific registrations of services found in the main program project
    }
}
```

Then, in program.cs, you can have the following code:

```csharp
using LasseVK.Bootstrapping;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Bootstrap(new ApplicationBootstrapper()); // this in turn calls the class library

var host = builder.Build();
await host.InitializeAsync();
await host.RunAsync();
```

For my own projects, this might be the entire Program.cs content, with all service
registrations moved to the `ApplicationBootstrapper` class, or into separate project
`ModuleBootstrapper` classes.

If anything is required to run against the `host` instance after building the host, but before
running the application, I try to move this into classes implementing the
`IModuleInitializer` interface. Again, I try to separate concerns into separate
class libraries with vertical slices of functionality.

Since calling `builder.Bootstrap(new SomeModuleBootstrapper())` will only invoke the
`Bootstrap` method on that type once per type (ie. once for `SomeModuleBootstrapper`),
any class library projects that also rely on other class libraries being registered
will all call the `Bootstrap` method with the respective module bootstrappers
from those dependency class libraries. This ensures all services are registered, even
if the main program project itself only require a few of them.

At most, you will construct additional instances of the bootstrapper classes on program
execution, but each `Bootstrap` method will only be invoked once.

# Maui applications

Maui applications do not follow the normal convention of implementing the
main host as a `IHost` derived type. There is a separate
nuget package for this: [LasseVK.Bootstrapping.Maui](https://www.nuget.org/packages/LasseVK.Bootstrapping.Maui/).
