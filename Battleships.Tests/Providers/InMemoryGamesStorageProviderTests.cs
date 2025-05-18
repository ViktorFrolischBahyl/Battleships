using Battleships.Models;
using Battleships.Models.Game;
using Battleships.Providers.Interfaces;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Battleships.Tests.Providers;

[TestClass]
public sealed class InMemoryGamesStorageProviderTests
{
    public InMemoryGamesStorageProviderTests()
    {
        var builder = Program.CreateHostBuilder(args: []);

        var host = builder.Build();

        this.GamesStorageProvider = ActivatorUtilities.GetServiceOrCreateInstance<IGamesStorageProvider>(host.Services);
        this.BattleshipsService = ActivatorUtilities.GetServiceOrCreateInstance<IBattleshipsService>(host.Services);
    }

    private IGamesStorageProvider GamesStorageProvider { get; }

    private IBattleshipsService BattleshipsService { get; }

    [TestMethod]
    public void InstanceTest()
    {
        var service = this.GamesStorageProvider;

        Assert.IsNotNull(service);
    }

    [TestMethod]
    public void StoreActiveGameTest()
    {
        var playerOne = new Player()
        {
            Name = "Player1"
        };

        var playerTwo = new Player()
        {
            Name = "Player2"
        };

        var game = new Game(playerOne, playerTwo);

        this.GamesStorageProvider.StoreActiveGame(game);

        var activeGame = this.GamesStorageProvider.GetActiveGame(game.GameId);

        Assert.IsNotNull(activeGame);
        Assert.AreEqual(game.GameId, activeGame.GameId);
    }

    [TestMethod]
    public void GetActiveGameTest()
    {
        var createdGame = this.CreateGame();

        var activeGame = this.GamesStorageProvider.GetActiveGame(createdGame.GameId);

        Assert.IsNotNull(activeGame);
        Assert.AreEqual(createdGame.GameId, activeGame.GameId);
    }

    [TestMethod]
    public void GetActiveGameNoSuchGameTest()
    {
        var exception = Assert.ThrowsException<KeyNotFoundException>(() => this.GamesStorageProvider.GetActiveGame("NO_SUCH_GAME"));

        Assert.IsNotNull(exception);
        Assert.IsTrue(exception.Message.Contains("Game with ID 'NO_SUCH_GAME' not found."));
    }

    [TestMethod]
    public void EndActiveGameTest()
    {
        var playerOne = new Player()
        {
            Name = "Player1"
        };

        var playerTwo = new Player()
        {
            Name = "Player2"
        };

        var game = new Game(playerOne, playerTwo);

        this.GamesStorageProvider.StoreActiveGame(game);

        this.GamesStorageProvider.EndActiveGame(game.GameId);

        var exception = Assert.ThrowsException<KeyNotFoundException>(() => this.GamesStorageProvider.GetActiveGame(game.GameId));

        Assert.IsNotNull(exception);
        Assert.IsTrue(exception.Message.Contains($"Game with ID '{game.GameId}' not found."));
    }

    [TestMethod]
    public void GetEndedGameTest()
    {
        var playerOne = new Player()
        {
            Name = "Player1"
        };

        var playerTwo = new Player()
        {
            Name = "Player2"
        };

        var game = new Game(playerOne, playerTwo);

        this.GamesStorageProvider.StoreActiveGame(game);

        this.GamesStorageProvider.EndActiveGame(game.GameId);

        var endedGame = this.GamesStorageProvider.GetEndedGame(game.GameId);

        Assert.IsNotNull(endedGame);
        Assert.AreEqual(game.GameId, endedGame.GameId);
    }

    [TestMethod]
    public void GetEndedGameNoSuchGameTest()
    {
        var exception = Assert.ThrowsException<KeyNotFoundException>(() => this.GamesStorageProvider.GetEndedGame("NO_SUCH_GAME"));

        Assert.IsNotNull(exception);
        Assert.IsTrue(exception.Message.Contains("Game with ID 'NO_SUCH_GAME' not found."));
    }

    private Game CreateGame()
    {
        var input = new CreateGameInput()
        {
            X = 10,
            Y = 10,
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
}