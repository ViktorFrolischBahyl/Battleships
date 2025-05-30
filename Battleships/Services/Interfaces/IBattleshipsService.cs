﻿using Battleships.Models;
using Battleships.Models.Game;

namespace Battleships.Services.Interfaces;

/// <summary>
/// Interface for the Battleships service, providing methods to manage battleship games.
/// </summary>
public interface IBattleshipsService
{
    /// <summary>
    /// Creates the game.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>Created game representation.</returns>
    public Game CreateGame(CreateGameInput input);

    /// <summary>
    /// Fires the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>Information about the game state after the fire action.</returns>
    public FireOutput Fire(FireInput input);
}