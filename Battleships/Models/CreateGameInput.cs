using System.ComponentModel.DataAnnotations;
using Battleships.Models.Game;

namespace Battleships.Models;

public class CreateGameInput
{
    [Required]
    public Player PlayerOne { get; set; }

    [Required]
    public Player PlayerTwo { get; set; }

    [Range(10, 20)]
    public int X { get; set; }

    [Range(10, 20)]
    public int Y { get; set; }
}