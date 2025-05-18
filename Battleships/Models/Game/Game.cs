namespace Battleships.Models.Game;

public class Game(Player playerOne, Player playerTwo)
{
    public string GameId { get; } = Guid.NewGuid().ToString("N");

    public Player PlayerOne { get; } = playerOne ?? throw new ArgumentNullException(nameof(playerOne));

    public Player PlayerTwo { get; } = playerTwo ?? throw new ArgumentNullException(nameof(playerTwo));

    public PlayingField? PlayerOneField { get; private set; }

    public PlayingField? PlayerTwoField { get; private set; }

    public Player NextMovePlayer { get; private set; } = playerOne ?? throw new ArgumentNullException(nameof(playerOne));

    public Player? Winner { get; private set; }

    public bool GameEnded => this.Winner != null;

    public void InitializeGame(List<Ship> shipsToPlace, Dimensions playingFieldDimensions)
    {
        _ = playingFieldDimensions ?? throw new ArgumentNullException(nameof(playingFieldDimensions));
        _ = shipsToPlace ?? throw new ArgumentNullException(nameof(shipsToPlace));

        this.PlayerOneField = new PlayingField(playingFieldDimensions);

        this.PlayerOneField.RandomlyPlaceShips(shipsToPlace);

        this.PlayerTwoField = new PlayingField(playingFieldDimensions);

        this.PlayerTwoField.RandomlyPlaceShips(shipsToPlace);
    }

    public PlayingField GetNextMovePlayerPlayingField()
    {
        if (this.NextMovePlayer.Name == this.PlayerOne.Name)
        {
            return this.PlayerTwoField ?? throw new InvalidOperationException($"Game was not initialized!");
        }

        if (this.NextMovePlayer.Name == this.PlayerTwo.Name)
        {
            return this.PlayerOneField ?? throw new InvalidOperationException($"Game was not initialized!");
        }

        throw new InvalidOperationException($"Player '{this.NextMovePlayer.Name}' does not have a playing field.");
    }

    public void ChangeNextMovePlayer()
    {
        if (this.NextMovePlayer.Name == this.PlayerOne.Name)
        {
            this.NextMovePlayer = this.PlayerTwo;
        }
        else if (this.NextMovePlayer.Name == this.PlayerTwo.Name)
        {
            this.NextMovePlayer = this.PlayerOne;
        }
        else
        {
            throw new InvalidOperationException($"Player '{this.NextMovePlayer.Name}' is not initialized player for this game.");
        }
    }

    public void EndGame()
    {
        this.Winner = this.NextMovePlayer;
    }
}