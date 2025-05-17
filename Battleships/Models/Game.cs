namespace Battleships.Models;

public class Game
{
    public Game(Player playerOne, Player playerTwo)
    {
        this.PlayerOne = playerOne;
        this.PlayerTwo = playerTwo;

        this.GameId = Guid.NewGuid().ToString("N");
    }

    public string GameId { get; }

    private Player PlayerOne { get; }

    private Player PlayerTwo { get; }

    private PlayingField PlayerOneField { get; set; }

    private PlayingField PlayerTwoField { get; set; }

    public void GeneratePlayersFields(List<Ship> shipsToRandomize, Dimensions playingFieldDimensions)
    {
        this.PlayerOneField = new PlayingField(playingFieldDimensions);
        
        this.PlayerOneField.RandomizeShips(shipsToRandomize);

        this.PlayerTwoField = new PlayingField(playingFieldDimensions);

        this.PlayerTwoField.RandomizeShips(shipsToRandomize);
    }
}