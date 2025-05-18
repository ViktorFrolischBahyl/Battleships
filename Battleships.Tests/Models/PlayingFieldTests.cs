using System.Diagnostics;
using Battleships.Models.Game;
using Battleships.Tests.Helpers;

namespace Battleships.Tests.Models;

[TestClass]
public sealed class PlayingFieldTests
{
    [TestMethod]
    public void RandomizeShipsTest()
    {
        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        var playingField = new PlayingField(playingFieldDimensions);

        Assert.IsNotNull(playingField);
        Assert.IsNotNull(playingField.PlayingFieldDimensions);
        Assert.IsNotNull(playingField.Grid);
        Assert.IsNotNull(playingField.Fleet);

        Assert.AreEqual(playingFieldDimensions.X, playingField.PlayingFieldDimensions.X);
        Assert.AreEqual(playingFieldDimensions.Y, playingField.PlayingFieldDimensions.Y);

        Assert.AreEqual(playingFieldDimensions.X, playingField.Grid.GetLength(0));
        Assert.AreEqual(playingFieldDimensions.Y, playingField.Grid.GetLength(1));

        Assert.AreEqual(playingFieldDimensions.X, playingField.Grid.Length / playingFieldDimensions.Y);
        Assert.AreEqual(playingFieldDimensions.Y, playingField.Grid.Length / playingFieldDimensions.X);

        Assert.AreEqual(0, playingField.Fleet.Count);

        foreach (var cell in playingField.Grid)
        {
            Assert.IsNotNull(cell);
            Assert.AreEqual(CellState.Water, cell.State);
        }

        var shipsToPlace = Helper.GetShipsToPlace();

        playingField.RandomlyPlaceShips(shipsToPlace);

        Assert.AreEqual(4, playingField.Fleet.Count);

        playingField.Fleet.ForEach(ship =>
        {
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship.Position);
            Assert.IsTrue(ship.Length > 0);
            Assert.IsTrue(ship.Position.Count > 0);
            Assert.IsTrue(ship.Length == ship.Position.Count);
            Assert.IsFalse(string.IsNullOrEmpty(ship.Type));

            ship.Position.ForEach(cell =>
            {
                Assert.IsNotNull(cell);
                Assert.AreEqual(CellState.Ship, cell.State);
            });
        });

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());
    }
}