# LasseVK.Bootstrapping.Maui

[![build](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/build.yml/badge.svg)](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/build.yml)
[![codeql](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/lassevk/LasseVK.Bootstrapping/actions/workflows/github-code-scanning/codeql)

This package is used in conjunction with the [LasseVK.Bootstrapping](https://www.nuget.org/packages/LasseVK.Bootstrapping)
package to handle initialization of Maui applications.

Maui applications do not follow the regular convention of implementing its application as an `IHost` implementation. Additionally,
the startup code of a Maui application is synchronous in nature, so asynchronous initialization is not possible.

## Discord

You can reach me through my [Discord server](https://discord.gg/xz2N2XZCV5).

## Nuget package

You can find the Nuget package [here](https://www.nuget.org/packages/LasseVK.Bootstrapping.Maui/).

## Installation

You can install the package from the command line, using `dotnet`:

```bash
dotnet add package LasseVK.Bootstrapping.Maui
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

See the main nuget package for usage instructions on how to set up bootstrappers and handle service registration.

When defining the initializer types for Maui applications, implement the `IMauiAppInitializer` interface:

```csharp
public interface IMauiAppInitializer
{
    void Initialize(MauiApp app);
}
```

Then in your main application you would typically have a `MauiProgram` class that, with the use of the bootstrapping
system and initialize support, might look like this:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.Bootstrap(new ApplicationBootstrapper());
        
        MauiApp app = builder.Build();
        app.Initialize();
        
        return app;
    }
}
```

The `ApplicationBootstrapper` class is a class that implements the `IApplicationBootstrapper<MauiAppBuilder>` or
the `IModuleBootstrapper` interface, and registers services, among other things implementations of `IMauiAppInitializer`.

Here are examples of these classes:

```csharp
public class ApplicationBootstrapper : IApplicationBootstrapper<MauiAppBuilder>
{
    public void Bootstrap(MauiAppBuilder builder)
    {
        builder
           .UseMauiApp<App>()
           .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddRadzenComponents();
            
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IMauiAppInitializer, DatabaseMigrationInitializer>();
    }
}

internal sealed class DatabaseMigrationInitializer : IMauiAppInitializer
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public DatabaseMigrationInitializer(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void Initialize(MauiApp app)
    {
        using AppDbContext context = _contextFactory.CreateDbContext();
        context.Database.Migrate();
    }
}
```

During startup, the program will first call the `Bootstrap` method of the `ApplicationBootstrapper` class, which will
register the services that are needed for the application to run, including the `IMauiAppInitializer` implementation.

In this case the initializer will run migrations as part of the application startup, before the main application code
is executed.

# Asynchronous initialization

The main nuget package allows for asynchronous initialization of modules and applications,
but unfortunately the startup code in a Maui application is synchronous, so these are not
supported.

Any attempt at invoking the asynchronous initialization methods using code like this:

```csharp
initializer.InitializeAsync(app).GetAwaiter().GetResult();
```

will deadlock the main thread, and the application will never start. As such, registering any
asynchronous initializers in a Maui application is not supported. Since this cannot be
detected at compile time, it will throw an `InvalidOperationException` at runtime.
