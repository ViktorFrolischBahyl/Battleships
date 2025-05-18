namespace Battleships.Models.Game;

public class Game(Player playerOne, Player playerTwo)
{
    public string GameId { get; } = Guid.NewGuid().ToString("N");

    public Player PlayerOne { get; } = playerOne ?? throw new ArgumentNullException(nameof(playerOne));

    public Player PlayerTwo { get; } = playerTwo ?? throw new ArgumentNullException(nameof(playerTwo));

    public PlayingField? PlayerOneField { get; private set; }

    public PlayingField? PlayerTwoField { get; private set; }

    public void GeneratePlayersFields(List<Ship> shipsToPlace, Dimensions playingFieldDimensions)
    {
        _ = playingFieldDimensions ?? throw new ArgumentNullException(nameof(playingFieldDimensions));
        _ = shipsToPlace ?? throw new ArgumentNullException(nameof(shipsToPlace));

        this.PlayerOneField = new PlayingField(playingFieldDimensions);

        this.PlayerOneField.RandomlyPlaceShips(shipsToPlace);

        this.PlayerTwoField = new PlayingField(playingFieldDimensions);

        this.PlayerTwoField.RandomlyPlaceShips(shipsToPlace);
    }
}