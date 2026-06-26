using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NSubstitute;

namespace LasseVK.Bootstrapping.Tests;

public class HostExtensionsTests
{
    [Test]
    public async Task InitializeAsync_NoInitializers_ReturnsSafely()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHost host = builder.Build();

        await host.InitializeAsync();
    }

    [Test]
    public async Task InitializeAsync_OneModuleInitializer_IsCalledOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IModuleInitializer initializer = Substitute.For<IModuleInitializer>();
        builder.Services.AddSingleton(initializer);

        IHost host = builder.Build();
        await host.InitializeAsync();

        await initializer.Received(1).InitializeAsync();
    }

    [Test]
    public async Task InitializeAsync_OneApplicationInitializer_IsCalledOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHostInitializer<IHost> initializer = Substitute.For<IHostInitializer<IHost>>();
        builder.Services.AddSingleton(initializer);

        IHost host = builder.Build();
        await host.InitializeAsync();

        await initializer.Received(1).InitializeAsync(host);
    }

    [Test]
    public void InitializeAsync_NullHost_ThrowsArgumentNullException()
    {
        IHost? host = null;
        Assert.Throws<ArgumentNullException>(() => host!.InitializeAsync().GetAwaiter().GetResult());
    }
}