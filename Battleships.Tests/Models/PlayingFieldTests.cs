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

        Assert.AreEqual(shipsToPlace.Count, playingField.Fleet.Count);

        playingField.Fleet.ForEach(ship =>
        {
            Assert.IsNotNull(ship);
            Assert.IsNotNull(ship.Position);
            Assert.IsNotNull(ship.Shape);
            Assert.IsTrue(ship.Position.Count > 0);
            Assert.IsTrue(ship.GetNumberOfShipCells() == ship.Position.Count);
            Assert.IsFalse(string.IsNullOrEmpty(ship.Type));

            ship.Position.ForEach(cell =>
            {
                Assert.IsNotNull(cell);
                Assert.AreEqual(CellState.Ship, cell.State);
            });
        });

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());
    }

    [TestMethod]
    public void FireAtWaterTest()
    {
        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        var playingField = new PlayingField(playingFieldDimensions);

        var shipsToPlace = Helper.GetShipsToPlace();

        playingField.RandomlyPlaceShips(shipsToPlace);

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Cell? waterCell = null;

        foreach (var cell in playingField.Grid)
        {
            if (cell.State == CellState.Water)
            {
                waterCell = cell;
                break;
            }
        }

        Assert.IsNotNull(waterCell);

        var updatedCell = playingField.Fire(waterCell);

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Assert.IsNotNull(updatedCell);
        Assert.AreEqual(CellState.Miss, updatedCell.State);

        var ship = playingField.Fleet.FindAll(ship => ship.Position.Contains(updatedCell));

        Assert.IsNotNull(ship);
        Assert.AreEqual(0, ship.Count);
    }

    [TestMethod]
    public void FireAtShipTest()
    {
        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        var playingField = new PlayingField(playingFieldDimensions);

        var shipsToPlace = Helper.GetShipsToPlace();

        playingField.RandomlyPlaceShips(shipsToPlace);

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Cell? shipCell = null;

        foreach (var cell in playingField.Grid)
        {
            if (cell.State == CellState.Ship)
            {
                shipCell = cell;
                break;
            }
        }

        Assert.IsNotNull(shipCell);

        var updatedCell = playingField.Fire(shipCell);

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Assert.IsNotNull(updatedCell);
        Assert.AreEqual(CellState.Hit, updatedCell.State);

        var ship = playingField.Fleet.FindAll(ship => ship.Position.Contains(updatedCell));

        Assert.IsNotNull(ship);
        Assert.AreEqual(1, ship.Count);
    }

    [TestMethod]
    public void DestroyWholeShipTest()
    {
        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        var playingField = new PlayingField(playingFieldDimensions);

        var shipsToPlace = Helper.GetShipsToPlace();

        playingField.RandomlyPlaceShips(shipsToPlace);

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Cell? shipCell = null;

        foreach (var cell in playingField.Grid)
        {
            if (cell.State == CellState.Ship)
            {
                shipCell = cell;
                break;
            }
        }

        Assert.IsNotNull(shipCell);

        var updatedCell = playingField.Fire(shipCell);

        Assert.IsNotNull(updatedCell);
        Assert.AreEqual(CellState.Hit, updatedCell.State);

        var ships = playingField.Fleet.FindAll(ship => ship.Position.Contains(updatedCell));

        Assert.IsNotNull(ships);
        Assert.AreEqual(1, ships.Count);

        var ship = ships.Single();

        Assert.IsNotNull(ship);

        ship.Position.ForEach(cell =>
        {
            if (cell.State != CellState.Hit)
            {
                playingField.Fire(cell);
            }
        });

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Assert.IsTrue(playingField.WasWholeShipDestroyed(updatedCell));
    }

    [TestMethod]
    public void DestroyEntireFleetTest()
    {
        var playingFieldDimensions = Helper.GetPlayingFieldDimensions();

        var playingField = new PlayingField(playingFieldDimensions);

        var shipsToPlace = Helper.GetShipsToPlace();

        playingField.RandomlyPlaceShips(shipsToPlace);

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        playingField.Fleet.ForEach(ship =>
        {
            ship.Position.ForEach(cell => playingField.Fire(cell));
        });

        Trace.WriteLine(playingField.GetStringRepresentationOfGrid());

        Assert.IsTrue(playingField.AreAllShipsDestroyed());
    }
}