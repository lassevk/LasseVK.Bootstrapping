using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

using NSubstitute;

namespace LasseVK.Bootstrapping.Maui.Tests;

public class MauiAppExtensionsTests
{
    [Fact]
    public void Initialize_NoInitializers_ReturnsSafely()
    {
        MauiApp app = BuildApp();

        app.Initialize();
    }

    [Fact]
    public void Initialize_OneInitializer_IsInvokedWithApp()
    {
        IMauiAppInitializer initializer = Substitute.For<IMauiAppInitializer>();
        MauiApp app = BuildApp(services => services.AddSingleton(initializer));

        app.Initialize();

        initializer.Received(1).Initialize(app);
    }

    [Fact]
    public void Initialize_MultipleInitializers_AllInvoked()
    {
        IMauiAppInitializer first = Substitute.For<IMauiAppInitializer>();
        IMauiAppInitializer second = Substitute.For<IMauiAppInitializer>();
        MauiApp app = BuildApp(services =>
        {
            services.AddSingleton(first);
            services.AddSingleton(second);
        });

        app.Initialize();

        first.Received(1).Initialize(app);
        second.Received(1).Initialize(app);
    }

    [Fact]
    public void Initialize_WithAsyncModuleInitializerRegistered_ThrowsInvalidOperationException()
    {
        IModuleInitializer moduleInitializer = Substitute.For<IModuleInitializer>();
        IMauiAppInitializer appInitializer = Substitute.For<IMauiAppInitializer>();
        MauiApp app = BuildApp(services =>
        {
            services.AddSingleton(moduleInitializer);
            services.AddSingleton(appInitializer);
        });

        Assert.Throws<InvalidOperationException>(() => app.Initialize());

        // The async initializers are unsupported, so it must bail out before
        // invoking any of the synchronous Maui initializers.
        appInitializer.DidNotReceive().Initialize(Arg.Any<MauiApp>());
    }

    private static MauiApp BuildApp(Action<IServiceCollection>? configureServices = null)
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder(useDefaults: false);
        configureServices?.Invoke(builder.Services);
        return builder.Build();
    }
}
