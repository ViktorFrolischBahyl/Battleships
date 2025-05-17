namespace Battleships.Models;

public class Ship
{
    public string Type { get; set; }

    public int Length { get; set; }

    public List<Cell> Position { get; } = new List<Cell>();
}