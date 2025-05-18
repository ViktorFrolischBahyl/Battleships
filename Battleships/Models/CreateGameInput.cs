using System.ComponentModel.DataAnnotations;
using Battleships.Models.Game;

namespace Battleships.Models;

public class CreateGameInput
{
    [Required]
    public Player PlayerOne { get; set; }

    [Required]
    public Player PlayerTwo { get; set; }

    [Required]
    public Dimensions PlayingFieldDimensions { get; set; }
}