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

    /// <summary>
    /// Gets or sets the games storage provider.
    /// </summary>
    /// <value>
    /// The games storage provider.
    /// </value>
    public GamesStorageProvider GamesStorageProvider { get; set; }
}