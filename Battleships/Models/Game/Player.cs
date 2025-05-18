using System.ComponentModel.DataAnnotations;

namespace Battleships.Models.Game;

/// <summary>
/// Model representing a player in the game.
/// </summary>
public class Player
{
    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    /// <value>
    /// The name of the player.
    /// </value>
    [Required]
    public string Name { get; set; }
}