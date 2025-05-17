using Battleships.Models;

namespace Battleships.Services.Interfaces;

public interface IBattleshipsService
{
    public Game CreateGame(CreateGameInput input);
}