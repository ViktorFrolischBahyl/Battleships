using Battleships.Models.Game;

namespace Battleships.Services.Interfaces;

/// <summary>
/// Interface for the service that loads ship definitions for the game.
/// </summary>
public interface ILoadShipsDefinitionsService
{
    /// <summary>
    /// Loads the ships definitions.
    /// </summary>
    /// <returns>List of ships to be placed on the playing field.</returns>
    public List<Ship> LoadShipsDefinitions();
}