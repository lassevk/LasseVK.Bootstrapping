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
    public async Task InitializeAsync_OneInitializer_IsCalledOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IModuleInitializer initializer = Substitute.For<IModuleInitializer>();
        builder.Services.AddSingleton(initializer);

        IHost host = builder.Build();
        await host.InitializeAsync();

        await initializer.Received(1).InitializeAsync();
    }

    [Test]
    public void InitializeAsync_NullHost_ThrowsArgumentNullException()
    {
        IHost? host = null;
        Assert.Throws<ArgumentNullException>(() => host!.InitializeAsync().GetAwaiter().GetResult());
    }
}