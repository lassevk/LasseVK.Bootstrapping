using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

/// <summary>
/// This interface must be implemented by a class in the host application and is responsible for configuring the host builder
/// before the host is built.
/// </summary>
/// <typeparam name="T">
/// The type of the host builder, implementing <see cref="IHostApplicationBuilder"/>.
/// </typeparam>
public interface IApplicationBootstrapper<in T>
    where T : IHostApplicationBuilder
{
    /// <summary>
    /// This method is called by the <see cref="HostApplicationBuilderExtensions.Bootstrap{T}(T,IApplicationBootstrapper{T})"/> method,
    /// once, for *type* of class implementing this interface.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IHostApplicationBuilder"/> to configure.
    /// </param>
    void Bootstrap(T builder);
}