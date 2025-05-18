namespace Battleships.Models.Game;

public class Cell : Dimensions
{
    public CellState State { get; set; } = CellState.Water;
}