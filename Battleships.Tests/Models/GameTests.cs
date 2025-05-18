using Battleships.Models.Game;
using Battleships.Tests.Helpers;
using System.Diagnostics;

namespace Battleships.Tests.Models;

[TestClass]
public sealed class GameTests
{
    [TestMethod]
    public void InitializeGameTest()
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

        Assert.IsNotNull(game);
        Assert.IsFalse(string.IsNullOrEmpty(game.GameId));

        Assert.IsNotNull(game.PlayerOne);
        Assert.IsNotNull(game.PlayerTwo);

        Assert.IsNull(game.PlayerOneField);
        Assert.IsNull(game.PlayerTwoField);

        var shipsToPlace = Helper.GetShipsToPlace();

        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        game.InitializeGame(shipsToPlace, playingFieldDimensions);
        
        Assert.IsNotNull(game.PlayerOneField);
        Assert.IsNotNull(game.PlayerTwoField);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());
    }

    [TestMethod]
    public void GetNextMovePlayerPlayingFieldTest()
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

        var shipsToPlace = Helper.GetShipsToPlace();

        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        game.InitializeGame(shipsToPlace, playingFieldDimensions);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());

        var nextMovePlayerPlayingField = game.GetNextMovePlayerPlayingField();

        Trace.WriteLine($"Next move player '{game.NextMovePlayer.Name}' playing field:");
        Trace.WriteLine(nextMovePlayerPlayingField.GetStringRepresentationOfGrid());

        Assert.AreEqual(game.NextMovePlayer.Name, game.PlayerOne.Name);
    }

    [TestMethod]
    public void ChangeNextMovePlayerTest()
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

        var shipsToPlace = Helper.GetShipsToPlace();

        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        game.InitializeGame(shipsToPlace, playingFieldDimensions);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());

        game.ChangeNextMovePlayer();

        var nextMovePlayerPlayingField = game.GetNextMovePlayerPlayingField();

        Trace.WriteLine($"Next move player '{game.NextMovePlayer.Name}' playing field:");
        Trace.WriteLine(nextMovePlayerPlayingField.GetStringRepresentationOfGrid());

        Assert.AreEqual(game.NextMovePlayer.Name, game.PlayerTwo.Name);
    }

    [TestMethod]
    public void ChangeNextMovePlayerTwoTimesTest()
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

        var shipsToPlace = Helper.GetShipsToPlace();

        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        game.InitializeGame(shipsToPlace, playingFieldDimensions);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());

        game.ChangeNextMovePlayer();

        game.ChangeNextMovePlayer();

        var nextMovePlayerPlayingField = game.GetNextMovePlayerPlayingField();

        Trace.WriteLine($"Next move player '{game.NextMovePlayer.Name}' playing field:");
        Trace.WriteLine(nextMovePlayerPlayingField.GetStringRepresentationOfGrid());

        Assert.AreEqual(game.NextMovePlayer.Name, game.PlayerOne.Name);
    }

    [TestMethod]
    public void EndGameTest()
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

        var shipsToPlace = Helper.GetShipsToPlace();

        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        game.InitializeGame(shipsToPlace, playingFieldDimensions);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());

        game.EndGame();

        Assert.IsNotNull(game.Winner);

        Assert.IsTrue(game.GameEnded);

        var nextMovePlayerPlayingField = game.GetNextMovePlayerPlayingField();

        Trace.WriteLine($"Winner '{game.Winner.Name}' playing field:");
        Trace.WriteLine(nextMovePlayerPlayingField.GetStringRepresentationOfGrid());

        Assert.AreEqual(game.Winner.Name, game.PlayerOne.Name);
    }

    [TestMethod]
    public void EndGameAfterChangeTest()
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

        var shipsToPlace = Helper.GetShipsToPlace();

        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        game.InitializeGame(shipsToPlace, playingFieldDimensions);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());

        game.ChangeNextMovePlayer();

        game.EndGame();

        Assert.IsNotNull(game.Winner);

        Assert.IsTrue(game.GameEnded);

        var nextMovePlayerPlayingField = game.GetNextMovePlayerPlayingField();

        Trace.WriteLine($"Winner '{game.Winner.Name}' playing field:");
        Trace.WriteLine(nextMovePlayerPlayingField.GetStringRepresentationOfGrid());

        Assert.AreEqual(game.Winner.Name, game.PlayerTwo.Name);
    }
}