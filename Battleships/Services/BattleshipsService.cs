using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Models.Settings;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Services;

/// <summary>
/// Implementation of the <see cref="IBattleshipsService" /> interface for managing battleship games.
/// </summary>
/// <seealso cref="Battleships.Services.Interfaces.IBattleshipsService" />
public class BattleshipsService(ILogger<BattleshipsService> logger, IOptions<ApplicationSettings> settings) : IBattleshipsService
{
    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <value>
    /// The settings.
    /// </value>
    private ApplicationSettings Settings { get; } = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    private ILogger<BattleshipsService> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Gets the active games.
    /// </summary>
    /// <value>
    /// The active games.
    /// </value>
    private Dictionary<string, Game> ActiveGames { get; } = [];

    /// <summary>
    /// Gets the finished games.
    /// </summary>
    /// <value>
    /// The finished games.
    /// </value>
    private Dictionary<string, Game> FinishedGames { get; } = [];

    /// <inheritdoc />
    public Game CreateGame(CreateGameInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        try
        {
            this.Logger.LogDebug($"Method {nameof(this.CreateGame)} started with following inputs: " +
                                 $"{nameof(input.X)}={input.X}, " +
                                 $"{nameof(input.Y)}={input.Y}, " +
                                 $"{nameof(input.PlayerOne)}={input.PlayerOne.Name}, " +
                                 $"{nameof(input.PlayerTwo)}={input.PlayerTwo.Name}");

            var game = new Game(
                playerOne: input.PlayerOne,
                playerTwo: input.PlayerTwo);

            this.Logger.LogTrace($"Initialization of game {game.GameId} started.");

            game.InitializeGame(
                this.Settings.Battleships,
                new Dimensions()
                {
                    X = input.X,
                    Y = input.Y,
                });

            this.Logger.LogTrace($"Initialization of game {game.GameId} finished.");

            this.ActiveGames.Add(game.GameId, game);

            this.Logger.LogDebug($"Method {nameof(this.CreateGame)} ended with game created {nameof(game.GameId)}={game.GameId}.");

            return game;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Method {nameof(this.CreateGame)} ended with error.");
            throw;
        }
    }

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
    public FireOutput Fire(FireInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        try
        {
            this.Logger.LogDebug($"Method {nameof(this.Fire)} started with following inputs: " +
                                 $"{nameof(input.GameId)}={input.GameId}, " +
                                 $"{nameof(input.CellDimensions.X)}={input.CellDimensions.X}, " +
                                 $"{nameof(input.CellDimensions.Y)}={input.CellDimensions.Y}");

            var game = this.GetActiveGame(input.GameId);

            var playingField = game.GetNextMovePlayerPlayingField();

            this.Logger.LogDebug($"Player '{game.NextMovePlayer.Name}' firing at (X:{input.CellDimensions.X}, Y:{input.CellDimensions.Y}).");

            var cellFiredAt = playingField.Fire(input.CellDimensions);

            var result = new FireOutput()
            {
                GameId = input.GameId,
            };

            if (cellFiredAt.State is CellState.Hit)
            {
                result.HitState = HitState.Hit;

                var wasWholeShipDestroyed = playingField.WasWholeShipDestroyed(cellFiredAt);

                if (wasWholeShipDestroyed)
                {
                    result.HitState = HitState.WholeShipDestroyed;

                    var areAllShipsDestroyed = playingField.AreAllShipsDestroyed();

                    if (areAllShipsDestroyed)
                    {
                        game.EndGame();

                        result.Winner = game.Winner;

                        this.FinishedGames.Add(game.GameId, game);
                        this.ActiveGames.Remove(game.GameId);
                    }
                }
            }
            else
            {
                result.HitState = HitState.Water;

                game.ChangeNextMovePlayer();
            }

            this.Logger.LogDebug($"Method {nameof(this.Fire)} ended with following output: " +
                                 $"{nameof(result.GameId)}={result.GameId}, " +
                                 $"{nameof(result.HitState)}={result.HitState}, " +
                                 $"{nameof(result.GameEnded)}={result.GameEnded}, " +
                                 $"{nameof(result.Winner)}={result.Winner?.Name}");

            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Method {nameof(this.Fire)} ended with error.");
            throw;
        }
    }
}