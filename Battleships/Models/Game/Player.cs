using System.ComponentModel.DataAnnotations;

namespace Battleships.Models.Game;

public class Player
{
    [Required]
    public string Name { get; set; }
}