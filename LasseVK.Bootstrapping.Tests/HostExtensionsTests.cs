using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NSubstitute;

namespace LasseVK.Bootstrapping.Tests;

public class HostExtensionsTests
{
    [Fact]
    public async Task InitializeAsync_NoInitializers_ReturnsSafely()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHost host = builder.Build();

        await host.InitializeAsync();
    }

    [Fact]
    public async Task InitializeAsync_OneModuleInitializer_IsCalledOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IModuleInitializer initializer = Substitute.For<IModuleInitializer>();
        builder.Services.AddSingleton(initializer);

        IHost host = builder.Build();
        await host.InitializeAsync();

        await initializer.Received(1).InitializeAsync();
    }

    [Fact]
    public async Task InitializeAsync_OneApplicationInitializer_IsCalledOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHostInitializer<IHost> initializer = Substitute.For<IHostInitializer<IHost>>();
        builder.Services.AddSingleton(initializer);

        IHost host = builder.Build();
        await host.InitializeAsync();

        await initializer.Received(1).InitializeAsync(host);
    }

    [Fact]
    public async Task InitializeAsync_InvokesModuleInitializersBeforeHostInitializers()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        var log = new List<string>();

        // Register the host initializer first to prove the order is driven by the
        // implementation, not by registration order.
        builder.Services.AddSingleton<IHostInitializer<IHost>>(new RecordingHostInitializer(log));
        builder.Services.AddSingleton<IModuleInitializer>(new RecordingModuleInitializer(log));

        IHost host = builder.Build();
        await host.InitializeAsync();

        Assert.Equal(new[] { "module", "host" }, log);
    }

    [Fact]
    public async Task InitializeAsync_ReturnsSameHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHost host = builder.Build();

        IHost result = await host.InitializeAsync();

        Assert.Same(host, result);
    }

    [Fact]
    public async Task InitializeAsync_NullHost_ThrowsArgumentNullException()
    {
        IHost? host = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() => host!.InitializeAsync());
    }

    private sealed class RecordingModuleInitializer : IModuleInitializer
    {
        private readonly List<string> _log;

        public RecordingModuleInitializer(List<string> log) => _log = log;

        public Task InitializeAsync()
        {
            _log.Add("module");
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingHostInitializer : IHostInitializer<IHost>
    {
        private readonly List<string> _log;

        public RecordingHostInitializer(List<string> log) => _log = log;

        public Task InitializeAsync(IHost host)
        {
            _log.Add("host");
            return Task.CompletedTask;
        }
    }
}
