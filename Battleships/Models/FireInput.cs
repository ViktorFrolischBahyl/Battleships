using Battleships.Models.Game;

namespace Battleships.Models;

/// <summary>
/// Model representing the input for firing a shot in the game.
/// </summary>
public class FireInput(string gameId, Dimensions cellDimensions)
{
    /// <summary>
    /// Gets the game identifier.
    /// </summary>
    /// <value>
    /// The game identifier.
    /// </value>
    public string GameId { get; } = gameId ?? throw new ArgumentNullException(nameof(gameId));

    /// <summary>
    /// Gets the dimensions of the cells that should be fired at.
    /// </summary>
    /// <value>
    /// The dimensions of the cells that should be fired at.
    /// </value>
    public Dimensions CellDimensions { get; } = cellDimensions ?? throw new ArgumentNullException(nameof(cellDimensions));
}