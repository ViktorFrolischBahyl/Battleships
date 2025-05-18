using System.ComponentModel.DataAnnotations;
using Battleships.Models.Game;

namespace Battleships.Models;

public class FireInput
{
    [Required]
    public string GameId { get; set; }

    [Required]
    public Dimensions CellDimensions { get; set; }
}