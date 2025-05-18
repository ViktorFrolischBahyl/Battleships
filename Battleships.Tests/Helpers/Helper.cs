using Battleships.Models.Game;

namespace Battleships.Tests.Helpers;

public static class Helper
{
    public static List<Ship> GetShipsToPlace()
    {
        var shipsToPlace = new List<Ship>()
        {
            new()
            {
                Length = 5,
                Type = "Carrier",
            },
            new()
            {
                Length = 1,
                Type = "Submarine",
            },
            new()
            {
                Length = 1,
                Type = "Submarine",
            },
            new()
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