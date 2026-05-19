using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

/// <summary>
/// This interface must be implemented by a class in a module (class library) and is responsible for registering
/// services and preparing the host.
/// </summary>
public interface IModuleBootstrapper
{
    /// <summary>
    /// This method is called by the <see cref="HostApplicationBuilderExtensions.Bootstrap"/> method,
    /// once, for *type* of class implementing this interface.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IHostApplicationBuilder"/> to configure and register services with.
    /// </param>
    void Bootstrap(IHostApplicationBuilder builder);
}