namespace Battleships.Models;

/// <summary>
/// Enum for the state of a hit in the game.
/// </summary>
public enum HitState
{
    /// <summary>
    /// The water
    /// </summary>
    Water,

    /// <summary>
    /// The hit
    /// </summary>
    Hit,

    /// <summary>
    /// The whole ship destroyed
    /// </summary>
    WholeShipDestroyed,
}