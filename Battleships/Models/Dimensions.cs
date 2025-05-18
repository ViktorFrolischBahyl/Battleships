using System.ComponentModel.DataAnnotations;

namespace Battleships.Models;

public class Dimensions
{
    [Range(10, 20)]
    public int X { get; set; }

    [Range(10, 20)]
    public int Y { get; set; }
}