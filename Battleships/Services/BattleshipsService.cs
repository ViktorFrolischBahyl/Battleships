using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Models.Settings;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Services;

public class BattleshipsService(IOptions<ApplicationSettings> settings) : IBattleshipsService
{
    private ApplicationSettings Settings { get; } = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    private Dictionary<string, Game> ActiveGames { get; } = [];

    private Dictionary<string, Game> FinishedGames { get; } = [];

    public Game CreateGame(CreateGameInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        var game = new Game(
            playerOne: input.PlayerOne,
            playerTwo: input.PlayerTwo);

        game.InitializeGame(
            this.Settings.Battleships,
            new Dimensions()
            {
                X = input.X,
                Y = input.Y,
            });

        this.ActiveGames.Add(game.GameId, game);

        return game;
    }

    public Game GetActiveGame(string gameId)
    {
        if (this.ActiveGames.TryGetValue(gameId, out var game))
        {
            return game;
        }

        throw new KeyNotFoundException($"Game with ID '{gameId}' not found.");
    }

    public FireOutput Fire(FireInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        var game = this.GetActiveGame(input.GameId);

        var playingField = game.GetNextMovePlayerPlayingField();

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

        return result;
    }
}