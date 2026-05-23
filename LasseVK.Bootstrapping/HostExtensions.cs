using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

/// <summary>
/// This class holds extension methods for <see cref="IHost"/>.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Initializes the host by invoking all implementations of <see cref="IModuleInitializer"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the host, implementing <see cref="IHost"/>.
    /// </typeparam>
    /// <param name="host">
    /// The <see cref="IHost"/> to invoke extension methods on.
    /// </param>
    /// <returns>
    /// The <paramref name="host"/> of the extension method.
    /// </returns>
    /// <remarks>
    /// The initializers are invoked sequentially, but order is not guaranteed, so no
    /// dependencies between initializers are guaranteed.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="host"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<T> InitializeAsync<T>(this T host)
        where T: IHost
    {
        ArgumentNullException.ThrowIfNull(host);

        var initializers = host.Services.GetServices<IModuleInitializer>().ToList();

        foreach (IModuleInitializer initializer in initializers)
        {
            await initializer.InitializeAsync();
        }

        return host;
    }
}