using Battleships.Models.Game;

namespace Battleships.Providers.Interfaces;

/// <summary>
/// Interface for the game storage provider, responsible for managing game state and data persistence.
/// </summary>
public interface IGamesStorageProvider
{
    /// <summary>
    /// Gets the active game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    /// <returns>Game representation of active game.</returns>
    public Game GetActiveGame(string gameId);

    /// <summary>
    /// Stores the active game.
    /// </summary>
    /// <param name="game">The game.</param>
    public void StoreActiveGame(Game game);

    /// <summary>
    /// Ends the active game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    public void EndActiveGame(string gameId);

    /// <summary>
    /// Gets the ended game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    /// <returns>Game representation of ended game.</returns>
    public Game GetEndedGame(string gameId);
}