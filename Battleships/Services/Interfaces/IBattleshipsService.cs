using Battleships.Models;
using Battleships.Models.Game;

namespace Battleships.Services.Interfaces;

public interface IBattleshipsService
{
    public Game CreateGame(CreateGameInput input);
}