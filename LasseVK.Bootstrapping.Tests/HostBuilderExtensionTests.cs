using Microsoft.Extensions.Hosting;

using NSubstitute;

namespace LasseVK.Bootstrapping.Tests;

public class HostBuilderExtensionTests
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
}