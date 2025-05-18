namespace Battleships.Models.Game;

public class Ship
{
    public string? Type { get; set; }

    public int Length { get; set; }

    public List<Cell> Position { get; } = [];
}