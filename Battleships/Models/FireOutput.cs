using Battleships.Models.Game;
using System.Text.Json.Serialization;

namespace Battleships.Models;

/// <summary>
/// Model representing the output of a fire action in the game.
/// </summary>
public class FireOutput
{
    /// <summary>
    /// Gets or sets the game identifier.
    /// </summary>
    /// <value>
    /// The game identifier.
    /// </value>
    public string GameId { get; set; }

    /// <summary>
    /// Gets or sets the state of the hit.
    /// </summary>
    /// <value>
    /// The state of the hit.
    /// </value>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HitState HitState { get; set; }

    /// <summary>
    /// Gets a value indicating whether the game ended.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the game ended; otherwise, <c>false</c>.
    /// </value>
    public bool GameEnded => this.Winner != null;

    /// <summary>
    /// Gets or sets the winner.
    /// </summary>
    /// <value>
    /// The winner.
    /// </value>
    public Player? Winner { get; set; } = null;
}