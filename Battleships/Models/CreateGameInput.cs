using System.ComponentModel.DataAnnotations;
using Battleships.Models.Game;

namespace Battleships.Models;

/// <summary>
/// Model representing the input for creating a new game.
/// </summary>
public class CreateGameInput
{
    /// <summary>
    /// Gets or sets the player one.
    /// </summary>
    /// <value>
    /// The player one.
    /// </value>
    [Required]
    public Player PlayerOne { get; set; }

    /// <summary>
    /// Gets or sets the player two.
    /// </summary>
    /// <value>
    /// The player two.
    /// </value>
    [Required]
    public Player PlayerTwo { get; set; }

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    [Range(10, 20)]
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    [Range(10, 20)]
    public int Y { get; set; }
}