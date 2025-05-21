using Battleships.Models.Game;

namespace Battleships.Models.Settings;

/// <summary>
/// Model representing application settings for the Battleships game.
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// Gets or sets the path to ships definition file.
    /// </summary>
    /// <value>
    /// The path to ships definition file.
    /// </value>
    public string PathToShipsDefinitionFile { get; set; }
}