using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Models.Settings;
using Battleships.Providers.Interfaces;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Services;

/// <summary>
/// Implementation of the <see cref="IBattleshipsService" /> interface for managing battleship games.
/// </summary>
/// <seealso cref="Battleships.Services.Interfaces.IBattleshipsService" />
public class BattleshipsService(
    ILogger<BattleshipsService> logger,
    ILoadShipsDefinitionsService loadShipsDefinitionsService,
    IGamesStorageProvider gamesStorageProvider)
    : IBattleshipsService
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    private ILogger<BattleshipsService> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Gets the games storage provider.
    /// </summary>
    /// <value>
    /// The games storage provider.
    /// </value>
    private IGamesStorageProvider GamesStorageProvider { get; } = gamesStorageProvider ?? throw new ArgumentNullException(nameof(gamesStorageProvider));

    /// <summary>
    /// Gets the load ships definitions service.
    /// </summary>
    /// <value>
    /// The load ships definitions service.
    /// </value>
    private ILoadShipsDefinitionsService LoadShipsDefinitionsService { get; } = loadShipsDefinitionsService ?? throw new ArgumentNullException(nameof(loadShipsDefinitionsService));

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
                this.LoadShipsDefinitionsService.LoadShipsDefinitions(),
                new Dimensions()
                {
                    X = input.X,
                    Y = input.Y,
                });

            this.Logger.LogTrace($"Initialization of game {game.GameId} finished.");

            this.GamesStorageProvider.StoreActiveGame(game);

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
    public FireOutput Fire(FireInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        try
        {
            this.Logger.LogDebug($"Method {nameof(this.Fire)} started with following inputs: " +
                                 $"{nameof(input.GameId)}={input.GameId}, " +
                                 $"{nameof(input.CellDimensions.X)}={input.CellDimensions.X}, " +
                                 $"{nameof(input.CellDimensions.Y)}={input.CellDimensions.Y}");

            var game = this.GamesStorageProvider.GetActiveGame(input.GameId);

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

                        this.GamesStorageProvider.EndActiveGame(game.GameId);
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