namespace LasseVK.Bootstrapping.Maui;

/// <summary>
/// This class holds extension methods for <see cref="MauiApp"/>.
/// </summary>
public static class MauiAppExtensions
{
    /// <param name="app">
    /// The <see cref="MauiApp"/> to invoke extension methods on.
    /// </param>
    extension(MauiApp app)
    {
        /// <summary>
        /// Initializes the application by invoking all implementations of <see cref="IMauiAppInitializer"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Async module initializers are not supported in Maui.
        /// </exception>
        public void Initialize()
        {
            var moduleInitializers = app.Services.GetServices<IModuleInitializer>().ToList();
            if (moduleInitializers.Any())
            {
                throw new InvalidOperationException("Async module initializers are not supported in Maui.");
            }

            var initializers = app.Services.GetServices<IMauiAppInitializer>().ToList();
            foreach (IMauiAppInitializer initializer in initializers)
            {
                initializer.Initialize(app);
            }
        }
    }
}