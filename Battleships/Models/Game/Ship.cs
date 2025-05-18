namespace Battleships.Models.Game;

/// <summary>
/// Model representing dimensions of the game grid.
/// </summary>
public class Ship
{
    /// <summary>
    /// Gets or sets the type of the ship.
    /// </summary>
    /// <value>
    /// The type of the ship.
    /// </value>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the length of the ship.
    /// </summary>
    /// <value>
    /// The length of the ship.
    /// </value>
    public int Length { get; set; }

    /// <summary>
    /// Gets the position of the ship on the grid.
    /// </summary>
    /// <value>
    /// The position of the ship on the grid.
    /// </value>
    public List<Cell> Position { get; } = [];
}