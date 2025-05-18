namespace Battleships.Models.Game;

public class Cell
{
    public int X { get; init; }

    public int Y { get; init; }

    public CellState State { get; set; } = CellState.Water;
}