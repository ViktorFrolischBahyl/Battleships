namespace Battleships.Models;

public class CreateGameInput
{
    public Player PlayerOne { get; set; }

    public Player PlayerTwo { get; set; }

    public Dimensions PlayingFieldDimensions { get; set; }
}