namespace Battleships.Providers.Interfaces;

/// <summary>
/// Interface for the game storage provider factory, responsible for creating instances of game storage providers.
/// </summary>
public interface IGamesStorageProviderFactory
{
    // TODO > ADD > other providers: file, database, etc.

    /// <summary>
    /// Gets the games storage provider.
    /// </summary>
    /// <returns>Registered games storage provider based on application settings.</returns>
    public IGamesStorageProvider GetProvider();
}