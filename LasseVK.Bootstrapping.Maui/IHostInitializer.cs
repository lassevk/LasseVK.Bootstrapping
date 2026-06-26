namespace LasseVK.Bootstrapping.Maui;

/// <summary>
/// This interface must be implemented by a class in the Maui application and is responsible for initializing the host.
/// </summary>
public interface IMauiAppInitializer
{
    /// <summary>
    /// Performs the initialization of the host.
    /// </summary>
    /// <param name="host">
    /// The <see cref="MauiApp"/> to initialize.
    /// </param>
    void Initialize(MauiApp host);
}