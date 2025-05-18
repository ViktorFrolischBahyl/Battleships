namespace Battleships.Models.Game;

public class Game
{
    public Game(Player playerOne, Player playerTwo)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;

        GameId = Guid.NewGuid().ToString("N");
    }

    public string GameId { get; }

    private Player PlayerOne { get; }

    private Player PlayerTwo { get; }

    private PlayingField? PlayerOneField { get; set; }

    private PlayingField? PlayerTwoField { get; set; }

    public void GeneratePlayersFields(List<Ship> shipsToRandomize, Dimensions playingFieldDimensions)
    {
        PlayerOneField = new PlayingField(playingFieldDimensions);

        PlayerOneField.RandomizeShips(shipsToRandomize);

        PlayerTwoField = new PlayingField(playingFieldDimensions);

        PlayerTwoField.RandomizeShips(shipsToRandomize);
    }
}