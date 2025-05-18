using Battleships.Models.Game;
using Battleships.Providers.Interfaces;

namespace Battleships.Providers;

/// <summary>
/// Implementation of the <see cref="IGamesStorageProvider" /> interface for in-memory game storage.
/// </summary>
/// <seealso cref="Battleships.Providers.Interfaces.IGamesStorageProvider" />
public class InMemoryGamesStorageProvider(ILogger<InMemoryGamesStorageProvider> logger)
    : IGamesStorageProvider
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    private ILogger<InMemoryGamesStorageProvider> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Gets the active games.
    /// </summary>
    /// <value>
    /// The active games.
    /// </value>
    private Dictionary<string, Game> ActiveGames { get; } = [];

    /// <summary>
    /// Gets the ended games.
    /// </summary>
    /// <value>
    /// The ended games.
    /// </value>
    private Dictionary<string, Game> EndedGames { get; } = [];

    /// <inheritdoc />
    public Game GetActiveGame(string gameId)
    {
        if (string.IsNullOrEmpty(gameId)) throw new ArgumentException("Value cannot be null or empty.", nameof(gameId));

        this.Logger.LogDebug($"Method {nameof(this.GetActiveGame)} started with following inputs: " +
                             $"{nameof(gameId)}={gameId}");

        if (this.ActiveGames.TryGetValue(gameId, out var game))
        {
            this.Logger.LogDebug($"Method {nameof(this.GetActiveGame)} ended with active game found {nameof(game.GameId)}={game.GameId}.");

            return game;
        }

        var ex = new KeyNotFoundException($"Game with ID '{gameId}' not found.");
        this.Logger.LogError(ex, $"Method {nameof(this.GetActiveGame)} ended with error.");
        throw ex;
    }

    /// <inheritdoc />
    public void StoreActiveGame(Game game)
    {
        _ = game ?? throw new ArgumentNullException(nameof(game));

        this.Logger.LogDebug($"Method {nameof(this.StoreActiveGame)} started with following inputs: " +
                             $"{nameof(game.GameId)}={game.GameId}");

        this.ActiveGames.Add(game.GameId, game);

        this.Logger.LogDebug($"Method {nameof(this.StoreActiveGame)} ended with game {nameof(game.GameId)}={game.GameId} stored.");
    }

    /// <inheritdoc />
    public void EndActiveGame(string gameId)
    {
        if (string.IsNullOrEmpty(gameId)) throw new ArgumentException("Value cannot be null or empty.", nameof(gameId));

        this.Logger.LogDebug($"Method {nameof(this.EndActiveGame)} started with following inputs: " +
                             $"{nameof(gameId)}={gameId}");

        var game = this.GetActiveGame(gameId);

        this.EndedGames.Add(gameId, game);

        this.ActiveGames.Remove(gameId);

        this.Logger.LogDebug($"Method {nameof(this.EndActiveGame)} ended with game {nameof(game.GameId)}={game.GameId} moved to ended games.");
    }

    /// <inheritdoc />
    public Game GetEndedGame(string gameId)
    {
        if (string.IsNullOrEmpty(gameId)) throw new ArgumentException("Value cannot be null or empty.", nameof(gameId));

        this.Logger.LogDebug($"Method {nameof(this.GetEndedGame)} started with following inputs: " +
                             $"{nameof(gameId)}={gameId}");

        if (this.EndedGames.TryGetValue(gameId, out var game))
        {
            this.Logger.LogDebug($"Method {nameof(this.GetEndedGame)} ended with active game found {nameof(game.GameId)}={game.GameId}.");

            return game;
        }

        var ex = new KeyNotFoundException($"Game with ID '{gameId}' not found.");
        this.Logger.LogError(ex, $"Method {nameof(this.GetEndedGame)} ended with error.");
        throw ex;
    }
}