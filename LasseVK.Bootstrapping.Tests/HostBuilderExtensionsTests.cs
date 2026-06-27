using Microsoft.Extensions.Hosting;

using NSubstitute;

namespace LasseVK.Bootstrapping.Tests;

public class HostBuilderExtensionsTests
{
    [Fact]
    public void Bootstrap_CalledOnce_CallsBootstrapper()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();
        builder.Bootstrap(bootstrapper);

        bootstrapper.Received().Bootstrap(builder);
    }

    [Fact]
    public void Bootstrap_CalledTwice_CallsModuleBootstrapperOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();
        builder.Bootstrap(bootstrapper);
        builder.Bootstrap(bootstrapper);

        bootstrapper.Received(1).Bootstrap(builder);
    }

    [Fact]
    public void Bootstrap_CalledTwice_CallsApplicationBootstrapperOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        IApplicationBootstrapper<IHostApplicationBuilder> bootstrapper = Substitute.For<IApplicationBootstrapper<IHostApplicationBuilder>>();
        builder.Bootstrap(bootstrapper);
        builder.Bootstrap(bootstrapper);

        bootstrapper.Received(1).Bootstrap(builder);
    }

    [Fact]
    public void Bootstrap_TwoInstancesOfSameType_InvokesBootstrapOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        var first = new RecordingModuleBootstrapper();
        var second = new RecordingModuleBootstrapper();

        builder.Bootstrap(first);
        builder.Bootstrap(second);

        // The registry keys on the bootstrapper *type*, so the second instance of the
        // same type must not be invoked again.
        Assert.Equal(1, first.InvocationCount);
        Assert.Equal(0, second.InvocationCount);
    }

    [Fact]
    public void Bootstrap_TwoDifferentTypes_InvokesBoth()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        var first = new RecordingModuleBootstrapper();
        var second = new OtherRecordingModuleBootstrapper();

        builder.Bootstrap(first);
        builder.Bootstrap(second);

        Assert.Equal(1, first.InvocationCount);
        Assert.Equal(1, second.InvocationCount);
    }

    [Fact]
    public void Bootstrap_ModuleBootstrapper_ReturnsSameBuilder()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();

        HostApplicationBuilder result = builder.Bootstrap(bootstrapper);

        Assert.Same(builder, result);
    }

    [Fact]
    public void Bootstrap_ApplicationBootstrapper_ReturnsSameBuilder()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IApplicationBootstrapper<HostApplicationBuilder> bootstrapper = Substitute.For<IApplicationBootstrapper<HostApplicationBuilder>>();

        HostApplicationBuilder result = builder.Bootstrap(bootstrapper);

        Assert.Same(builder, result);
    }

    [Fact]
    public void Bootstrap_NullBuilder_ThrowsArgumentNullException()
    {
        IHostApplicationBuilder? builder = null;
        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();

        Assert.Throws<ArgumentNullException>(() => builder!.Bootstrap(bootstrapper));
    }

    [Fact]
    public void Bootstrap_NullModuleBootstrapper_ThrowsArgumentNullException()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.Bootstrap((IModuleBootstrapper)null!));
    }

    [Fact]
    public void Bootstrap_NullApplicationBootstrapper_ThrowsArgumentNullException()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.Bootstrap((IApplicationBootstrapper<IHostApplicationBuilder>)null!));
    }

    private sealed class RecordingModuleBootstrapper : IModuleBootstrapper
    {
        public int InvocationCount { get; private set; }

        public void Bootstrap(IHostApplicationBuilder builder) => InvocationCount++;
    }

    private sealed class OtherRecordingModuleBootstrapper : IModuleBootstrapper
    {
        public int InvocationCount { get; private set; }

        public void Bootstrap(IHostApplicationBuilder builder) => InvocationCount++;
    }
}
