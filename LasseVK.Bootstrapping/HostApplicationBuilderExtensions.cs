using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

/// <summary>
/// This class holds extension methods for <see cref="IHostApplicationBuilder"/>.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    private static readonly object _registryKey = new();

    /// <summary>
    /// Extension methods for <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IHostApplicationBuilder"/> to invoke extension methods on.
    /// </param>
    /// <typeparam name="T">
    /// The type of the builder, implementing <see cref="IHostApplicationBuilder"/>.
    /// </typeparam>
    extension<T>(T builder)
        where T : IHostApplicationBuilder
    {
        /// <summary>
        /// Invokes the <see cref="IModuleBootstrapper.Bootstrap"/> method
        /// of the provided bootstrapper once. If the method has already been
        /// invoked once for the type of the bootstrapper, the method is not
        /// invoked again.
        /// </summary>
        /// <param name="bootstrapper">
        /// The <see cref="IModuleBootstrapper"/> to invoke the <see cref="IModuleBootstrapper.Bootstrap"/> method on.
        /// </param>
        /// <returns>
        /// The <paramref name="builder"/> of the extension method.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> or <paramref name="bootstrapper"/> is <see langword="null"/>.
        /// </exception>
        public T Bootstrap(IModuleBootstrapper bootstrapper)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(bootstrapper);

            HashSet<Type> registry = builder.GetOrCreateRegistry();
            if (registry.Add(bootstrapper.GetType()))
            {
                bootstrapper.Bootstrap(builder);
            }

            return builder;
        }

        private HashSet<Type> GetOrCreateRegistry()
        {
            if (builder.Properties.TryGetValue(_registryKey, out object? registryObject))
            {
                return (HashSet<Type>)registryObject;
            }

            var registry = new HashSet<Type>();
            builder.Properties.Add(_registryKey, registry);
            return registry;
        }
    }
}