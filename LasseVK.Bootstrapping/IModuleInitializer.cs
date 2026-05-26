using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

/// <summary>
/// This interface must be implemented by a class in a module (class library) and is responsible for initializing
/// services and configuring the host.
/// </summary>
public interface IModuleInitializer
{
    /// <summary>
    /// This method is called by the <see cref="HostExtensions.InitializeAsync"/> method.
    /// </summary>
    /// <param name="host">
    /// The host to initialize.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    Task InitializeAsync(object host);
}