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

        AssertGame(createdGame);
    }

    [TestMethod]
    public void GetActiveGameTest()
    {
        var createdGame = this.CreateGame();

        var activeGame = this.BattleshipsService.GetActiveGame(createdGame.GameId);

        AssertGame(activeGame);
    }

    [TestMethod]
    public void GetActiveGameNoSuchGameTest()
    {
        var exception = Assert.ThrowsException<KeyNotFoundException>(() => this.BattleshipsService.GetActiveGame("NO_SUCH_GAME"));

        Assert.IsNotNull(exception);
        Assert.IsTrue(exception.Message.Contains("Game with ID 'NO_SUCH_GAME' not found."));
    }

    [TestMethod]
    public void FireAtWaterTest()
    {
        var createdGame = this.CreateGame();

        DisplayCurrentGameState(createdGame);

        var nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        var waterCell = nextPlayerPlayingField.Grid.Cast<Cell>().FirstOrDefault(cell => cell.State == CellState.Water);

        Assert.IsNotNull(waterCell);

        var input = new FireInput(createdGame.GameId, waterCell);

        Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{waterCell.X}, Y:{waterCell.Y}).");

        var fireOutput = this.BattleshipsService.Fire(input);

        Assert.IsNotNull(fireOutput);
        Assert.IsFalse(string.IsNullOrEmpty(fireOutput.GameId));
        Assert.AreEqual(createdGame.GameId, fireOutput.GameId);
        Assert.AreEqual(HitState.Water, fireOutput.HitState);

        Assert.AreEqual(createdGame.PlayerTwo.Name, createdGame.NextMovePlayer.Name);

        DisplayCurrentGameState(createdGame);
    }

    [TestMethod]
    public void FireAtShipTest()
    {
        var createdGame = this.CreateGame();

        DisplayCurrentGameState(createdGame);

        var nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        var shipToFireAt = nextPlayerPlayingField.Fleet.Find(ship => ship.Length > 1);

        Assert.IsNotNull(shipToFireAt);

        var shipCell = shipToFireAt.Position.First();

        Assert.IsNotNull(shipCell);

        var input = new FireInput(createdGame.GameId, shipCell);

        Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{shipCell.X}, Y:{shipCell.Y}).");

        var fireOutput = this.BattleshipsService.Fire(input);

        Assert.IsNotNull(fireOutput);
        Assert.IsFalse(string.IsNullOrEmpty(fireOutput.GameId));
        Assert.AreEqual(createdGame.GameId, fireOutput.GameId);
        Assert.AreEqual(HitState.Hit, fireOutput.HitState);

        Assert.AreEqual(createdGame.PlayerOne.Name, createdGame.NextMovePlayer.Name);

        DisplayCurrentGameState(createdGame);
    }

    [TestMethod]
    public void PlayerOneDestroysShipTest()
    {
        var createdGame = this.CreateGame();

        DisplayCurrentGameState(createdGame);

        var nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        var shipToFireAt = nextPlayerPlayingField.Fleet.Find(ship => ship.Length > 1);

        Assert.IsNotNull(shipToFireAt);

        FireOutput? fireOutput = null;

        shipToFireAt.Position.ForEach(cell =>
        {
            var input = new FireInput(createdGame.GameId, cell);

            Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{cell.X}, Y:{cell.Y}).");

            fireOutput = this.BattleshipsService.Fire(input);
        });

        Assert.IsNotNull(fireOutput);
        Assert.IsFalse(string.IsNullOrEmpty(fireOutput.GameId));
        Assert.AreEqual(createdGame.GameId, fireOutput.GameId);
        Assert.AreEqual(HitState.WholeShipDestroyed, fireOutput.HitState);

        Assert.AreEqual(createdGame.PlayerOne.Name, createdGame.NextMovePlayer.Name);

        DisplayCurrentGameState(createdGame);
    }

    [TestMethod]
    public void PlayerTwoDestroysShipTest()
    {
        var createdGame = this.CreateGame();

        DisplayCurrentGameState(createdGame);

        var nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        var waterCell = nextPlayerPlayingField.Grid.Cast<Cell>().FirstOrDefault(cell => cell.State == CellState.Water);

        Assert.IsNotNull(waterCell);

        var input = new FireInput(createdGame.GameId, waterCell);

        Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{waterCell.X}, Y:{waterCell.Y}).");

        this.BattleshipsService.Fire(input);

        nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        var shipToFireAt = nextPlayerPlayingField.Fleet.Find(ship => ship.Length > 1);

        Assert.IsNotNull(shipToFireAt);

        FireOutput? fireOutput = null;

        shipToFireAt.Position.ForEach(cell =>
        {
            input = new FireInput(createdGame.GameId, cell);

            Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{cell.X}, Y:{cell.Y}).");

            fireOutput = this.BattleshipsService.Fire(input);
        });

        Assert.IsNotNull(fireOutput);
        Assert.IsFalse(string.IsNullOrEmpty(fireOutput.GameId));
        Assert.AreEqual(createdGame.GameId, fireOutput.GameId);
        Assert.AreEqual(HitState.WholeShipDestroyed, fireOutput.HitState);

        Assert.AreEqual(createdGame.PlayerTwo.Name, createdGame.NextMovePlayer.Name);

        DisplayCurrentGameState(createdGame);
    }

    [TestMethod]
    public void PlayerOneWinsTest()
    {
        var createdGame = this.CreateGame();

        DisplayCurrentGameState(createdGame);

        var nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        FireOutput? fireOutput = null;

        nextPlayerPlayingField.Fleet.ForEach(ship =>
        {
            ship.Position.ForEach(cell =>
            {
                var input = new FireInput(createdGame.GameId, cell);

                Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{cell.X}, Y:{cell.Y}).");

                fireOutput = this.BattleshipsService.Fire(input);
            });
        });

        Assert.IsNotNull(fireOutput);
        Assert.IsFalse(string.IsNullOrEmpty(fireOutput.GameId));
        Assert.AreEqual(createdGame.GameId, fireOutput.GameId);
        Assert.AreEqual(HitState.WholeShipDestroyed, fireOutput.HitState);
        Assert.IsTrue(fireOutput.GameEnded);

        Assert.IsNotNull(createdGame.Winner);
        Assert.AreEqual(createdGame.PlayerOne.Name, createdGame.Winner.Name);

        DisplayCurrentGameState(createdGame);
    }

    [TestMethod]
    public void PlayerTwoWinsTest()
    {
        var createdGame = this.CreateGame();

        DisplayCurrentGameState(createdGame);

        var nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        var waterCell = nextPlayerPlayingField.Grid.Cast<Cell>().FirstOrDefault(cell => cell.State == CellState.Water);

        Assert.IsNotNull(waterCell);

        var input = new FireInput(createdGame.GameId, waterCell);

        Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{waterCell.X}, Y:{waterCell.Y}).");

        this.BattleshipsService.Fire(input);

        nextPlayerPlayingField = createdGame.GetNextMovePlayerPlayingField();

        FireOutput? fireOutput = null;

        nextPlayerPlayingField.Fleet.ForEach(ship =>
        {
            ship.Position.ForEach(cell =>
            {
                input = new FireInput(createdGame.GameId, cell);

                Trace.WriteLine($"Player '{createdGame.NextMovePlayer.Name}' firing at (X:{cell.X}, Y:{cell.Y}).");

                fireOutput = this.BattleshipsService.Fire(input);
            });
        });

        Assert.IsNotNull(fireOutput);
        Assert.IsFalse(string.IsNullOrEmpty(fireOutput.GameId));
        Assert.AreEqual(createdGame.GameId, fireOutput.GameId);
        Assert.AreEqual(HitState.WholeShipDestroyed, fireOutput.HitState);
        Assert.IsTrue(fireOutput.GameEnded);

        Assert.IsNotNull(createdGame.Winner);
        Assert.AreEqual(createdGame.PlayerTwo.Name, createdGame.Winner.Name);

        DisplayCurrentGameState(createdGame);
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

    private static void AssertGame(Game game)
    {
        Assert.IsNotNull(game);
        Assert.IsFalse(string.IsNullOrEmpty(game.GameId));

        Assert.IsNotNull(game.PlayerOne);
        Assert.IsNotNull(game.PlayerTwo);

        Assert.IsNotNull(game.PlayerOneField);
        Assert.IsNotNull(game.PlayerTwoField);

        DisplayCurrentGameState(game);
    }

    private static void DisplayCurrentGameState(Game game)
    {
        Trace.WriteLine($"Player '{game.PlayerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{game.PlayerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());
    }
}