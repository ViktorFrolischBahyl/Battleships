namespace Battleships.Models.Game;

/// <summary>
/// Model representing dimensions of the game grid.
/// </summary>
public class Ship(Cell[,] shape)
{
    /// <summary>
    /// Gets or sets the type of the ship.
    /// </summary>
    /// <value>
    /// The type of the ship.
    /// </value>
    public string? Type { get; set; }

    /// <summary>
    /// Gets the position of the ship on the grid.
    /// </summary>
    /// <value>
    /// The position of the ship on the grid.
    /// </value>
    public List<Cell> Position { get; } = [];

    /// <summary>
    /// Gets the shape of the ship.
    /// </summary>
    /// <value>
    /// The shape of the ship.
    /// </value>
    public Cell[,] Shape { get; } = shape ?? throw new ArgumentNullException(nameof(shape));

    /// <summary>
    /// Gets the number of ship cells.
    /// </summary>
    /// <returns>Count of cells in shape that are of type ship.</returns>
    public int GetNumberOfShipCells()
    {
        var numberOfShipCells = 0;

        foreach (var cell in this.Shape)
        {
            if (cell.State is CellState.Ship)
            {
                numberOfShipCells++;
            }
        }

        return numberOfShipCells;
    }
}