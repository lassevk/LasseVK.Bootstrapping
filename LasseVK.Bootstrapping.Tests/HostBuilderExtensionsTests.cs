using Microsoft.Extensions.Hosting;

using NSubstitute;

namespace LasseVK.Bootstrapping.Tests;

public class HostBuilderExtensionsTests
{
    [Test]
    public void Bootstrap_CalledOnce_CallsBootstrapper()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();
        builder.Bootstrap(bootstrapper);

        bootstrapper.Received().Bootstrap(builder);
    }

    [Test]
    public void Bootstrap_CalledTwice_CallsBootstrapperOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();
        builder.Bootstrap(bootstrapper);
        builder.Bootstrap(bootstrapper);

        bootstrapper.Received(1).Bootstrap(builder);
    }

    [Test]
    public void Bootstrap_NullBuilder_ThrowsArgumentNullException()
    {
        IHostApplicationBuilder? builder = null;
        IModuleBootstrapper bootstrapper = Substitute.For<IModuleBootstrapper>();

        Assert.Throws<ArgumentNullException>(() => builder!.Bootstrap(bootstrapper));
    }

    [Test]
    public void Bootstrap_NullBootstrapper_ThrowsArgumentNullException()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.Bootstrap(null!));
    }
}