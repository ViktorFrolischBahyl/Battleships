using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Models.Settings;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Services;

public class BattleshipsService(IOptions<ApplicationSettings> settings) : IBattleshipsService
{
    private ApplicationSettings Settings { get; } = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    private Dictionary<string, Game> ActiveGames { get; } = new();

    public Game CreateGame(CreateGameInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        var game = new Game(
            playerOne: input.PlayerOne,
            playerTwo: input.PlayerTwo);

        game.GeneratePlayersFields(
            this.Settings.Battleships,
            input.PlayingFieldDimensions);

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
}