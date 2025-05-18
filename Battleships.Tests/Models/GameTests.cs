using Battleships.Models.Game;
using Battleships.Tests.Helpers;
using System.Diagnostics;

namespace Battleships.Tests.Models;

[TestClass]
public sealed class GameTests
{
    [TestMethod]
    public void GeneratePlayersFieldsTest()
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

        game.GeneratePlayersFields(shipsToPlace, playingFieldDimensions);
        
        Assert.IsNotNull(game.PlayerOneField);
        Assert.IsNotNull(game.PlayerTwoField);

        Trace.WriteLine($"Player '{playerOne.Name}' playing field:");
        Trace.WriteLine(game.PlayerOneField.GetStringRepresentationOfGrid());

        Trace.WriteLine($"Player '{playerTwo.Name}' playing field:");
        Trace.WriteLine(game.PlayerTwoField.GetStringRepresentationOfGrid());
    }
}