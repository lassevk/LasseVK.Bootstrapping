namespace LasseVK.Bootstrapping.Maui;

/// <summary>
/// This interface must be implemented by a class in the Maui application and is responsible for initializing the application
/// before it starts.
/// </summary>
public interface IMauiAppInitializer
{
    /// <summary>
    /// Performs the initialization of the application.
    /// </summary>
    /// <param name="app">
    /// The <see cref="MauiApp"/> to initialize.
    /// </param>
    void Initialize(MauiApp app);
}