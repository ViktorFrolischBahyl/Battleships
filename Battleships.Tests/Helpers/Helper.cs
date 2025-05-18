using Battleships.Models.Game;

namespace Battleships.Tests.Helpers;

public static class Helper
{
    public static List<Ship> GetShipsToPlace()
    {
        var shipsToPlace = new List<Ship>()
        {
            new Ship()
            {
                Length = 5,
                Type = "Carrier",
            },
            new Ship()
            {
                Length = 1,
                Type = "Submarine",
            },
            new Ship()
            {
                Length = 1,
                Type = "Submarine",
            },
            new Ship()
            {
                Length = 2,
                Type = "Boat",
            },
        };

        return shipsToPlace;
    }

    public static Dimensions GetPlayingFieldDimensions()
    {
        return new Dimensions()
        {
            X = 10,
            Y = 10,
        };
    }
}