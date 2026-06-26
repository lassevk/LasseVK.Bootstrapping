using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

/// <summary>
/// This interface must be implemented by a class in the host application and is responsible for initializing the host before
/// the host is asked to run.
/// </summary>
/// <typeparam name="T">
/// The type of the host, implementing <see cref="IHost"/>.
/// </typeparam>
public interface IHostInitializer<in T>
    where T : IHost
{
    /// <summary>
    /// Performs the initialization of the host, called by the <see cref="HostExtensions.InitializeAsync"/> method.
    /// </summary>
    /// <param name="host">
    /// The <see cref="IHost"/> to initialize.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    Task InitializeAsync(T host);
}