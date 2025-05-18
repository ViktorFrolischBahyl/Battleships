using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Battleships.Tests.Services;

[TestClass]
public sealed class BattleshipsServiceTests
{
    public BattleshipsServiceTests()
    {
        var builder = Program.CreateHostBuilder(args: []);

        var host = builder.Build();

        this.BattleshipsService = ActivatorUtilities.GetServiceOrCreateInstance<IBattleshipsService>(host.Services);
    }

    private IBattleshipsService BattleshipsService { get; }

    [TestMethod]
    public void InstanceTest()
    {
        var service = this.BattleshipsService;

        Assert.IsNotNull(service);
    }

    [TestMethod]
    public void CreateGameTest()
    {
        var createdGame = this.CreateGame();

        this.AssertGame(createdGame);
    }

    [TestMethod]
    public void GetActiveGameTest()
    {
        var createdGame = this.CreateGame();

        var activeGame = this.BattleshipsService.GetActiveGame(createdGame.GameId);

        this.AssertGame(activeGame);
    }

    [TestMethod]
    public void GetActiveGameNoSuchGameTest()
    {
        var exception = Assert.ThrowsException<KeyNotFoundException>(() => this.BattleshipsService.GetActiveGame("NO_SUCH_GAME"));

        Assert.IsNotNull(exception);
        Assert.IsTrue(exception.Message.Contains("Game with ID 'NO_SUCH_GAME' not found."));
    }

    private Game CreateGame()
    {
        var input = new CreateGameInput()
        {
            PlayingFieldDimensions = new Dimensions()
            {
                X = 10,
                Y = 10,
            },
            PlayerOne = new Player()
            {
                Name = "Player1",
            },
            PlayerTwo = new Player()
            {
                Name = "Player2",
            }
        };

        var createdGame = this.BattleshipsService.CreateGame(input);

        return createdGame;
    }

    private void AssertGame(Game game)
    {
        Assert.IsNotNull(game);
        Assert.IsFalse(string.IsNullOrEmpty(game.GameId));

        Assert.IsNotNull(game.PlayerOne);
        Assert.IsNotNull(game.PlayerTwo);

        Assert.IsNotNull(game.PlayerOneField);
        Assert.IsNotNull(game.PlayerTwoField);

        Trace.WriteLine($"Player '{game.PlayerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{game.PlayerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());
    }
}