namespace Battleships.Models.Game;

/// <summary>
/// Enum for cell state in the game grid.
/// </summary>
public enum CellState
{
    /// <summary>
    /// The water
    /// </summary>
    Water,

    /// <summary>
    /// The ship
    /// </summary>
    Ship,

    /// <summary>
    /// The hit
    /// </summary>
    Hit,

    /// <summary>
    /// The miss
    /// </summary>
    Miss,
}