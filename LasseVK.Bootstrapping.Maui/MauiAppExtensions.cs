namespace LasseVK.Bootstrapping.Maui;

/// <summary>
/// This class holds extension methods for <see cref="MauiApp"/>.
/// </summary>
public static class MauiAppExtensions
{
    /// <summary>
    /// Initializes the application by invoking all implementations of <see cref="IMauiAppInitializer"/>.
    /// </summary>
    /// <param name="app">
    /// The <see cref="MauiApp"/> to invoke extension methods on.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Async module initializers are not supported in Maui.
    /// </exception>
    public static void Initialize(this MauiApp app)
    {
        var moduleInitializers = app.Services.GetServices<IModuleInitializer>().ToList();
        if (moduleInitializers.Count != 0)
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