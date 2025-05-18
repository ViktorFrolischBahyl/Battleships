using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Models.Settings;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Services;

public class BattleshipsService : IBattleshipsService
{
    public BattleshipsService(IOptions<ApplicationSettings> settings)
    {
        this.Settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    private ApplicationSettings Settings { get; }

    private Dictionary<string, Game> ActiveGames { get; } = new ();

    public Game CreateGame(CreateGameInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        var game = new Game(
            playerOne: input.PlayerOne,
            playerTwo: input.PlayerTwo);

        game.GeneratePlayersFields(this.Settings.Battleships, input.PlayingFieldDimensions);

        this.ActiveGames.Add(game.GameId, game);

        return game;
    }
}