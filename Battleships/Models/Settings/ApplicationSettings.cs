using Battleships.Models.Game;

namespace Battleships.Models.Settings;

/// <summary>
/// Model representing application settings for the Battleships game.
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// Gets the battleships that should be generated on the playing fields.
    /// </summary>
    /// <value>
    /// The battleships that should be generated on the playing fields.
    /// </value>
    public List<Ship> Battleships { get; } = [];
}