namespace Battleships.Models.Game;

/// <summary>
/// Model representing a cell in the game grid.
/// </summary>
/// <seealso cref="Battleships.Models.Game.Dimensions" />
public class Cell : Dimensions
{
    /// <summary>
    /// Gets or sets the state of current cell.
    /// </summary>
    /// <value>
    /// The state of current cell.
    /// </value>
    public CellState State { get; set; } = CellState.Water;
}