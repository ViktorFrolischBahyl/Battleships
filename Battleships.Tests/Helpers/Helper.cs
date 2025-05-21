using Battleships.Models.Game;

namespace Battleships.Tests.Helpers;

public static class Helper
{
    public static List<Ship> GetShipsToPlace()
    {
        var shipsToPlace = new List<Ship>()
        {
            new(GenerateShape(3,1))
            {
                Type = "Carrier",
            },
            new(GenerateShape(1,1))
            {
                Type = "Submarine",
            },
            new(GenerateShape(1,1))
            {
                Type = "Submarine",
            },
            new(GenerateShape(2,1))
            {
                Type = "Boat",
            },
            new(GenerateShape(2,1))
            {
                Type = "Boat",
            },
            new(GeneratePlusShape())
            {
                Type = "Plus",
            },
            new(GenerateLShape())
            {
                Type = "L",
            },
        };

        return shipsToPlace;
    }

    public static Cell[,] GenerateShape(int x, int y)
    {
        var result = new Cell[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                result[i, j] = new Cell()
                {
                    State = CellState.Ship,
                };
            }
        }

        return result;
    }

    public static Cell[,] GeneratePlusShape()
    {
        var result = new Cell[3, 3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                result[x, y] = new Cell()
                {
                    State = CellState.Water,
                };

                if (x == 1 || y == 1)
                {
                    result[x, y].State = CellState.Ship;
                }
            }
        }

        return result;
    }

    public static Cell[,] GenerateLShape()
    {
        var result = new Cell[4, 2];

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                result[x, y] = new Cell()
                {
                    State = CellState.Water,
                };

                if (y == 0 || x == 0)
                {
                    result[x, y].State = CellState.Ship;
                }
            }
        }

        return result;
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