using Battleships.Models.Game;

namespace Battleships.Models;

public class FireInput(string gameId, Dimensions cellDimensions)
{
    public string GameId { get; } = gameId ?? throw new ArgumentNullException(nameof(gameId));

    public Dimensions CellDimensions { get; } = cellDimensions ?? throw new ArgumentNullException(nameof(cellDimensions));
}