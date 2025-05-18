namespace Battleships.Models.Game;

/// <summary>
/// Model representing dimensions of the game grid.
/// </summary>
public class Game(Player playerOne, Player playerTwo)
{
    /// <summary>
    /// Gets the game identifier.
    /// </summary>
    /// <value>
    /// The game identifier.
    /// </value>
    public string GameId { get; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the player one.
    /// </summary>
    /// <value>
    /// The player one.
    /// </value>
    public Player PlayerOne { get; } = playerOne ?? throw new ArgumentNullException(nameof(playerOne));

    /// <summary>
    /// Gets the player two.
    /// </summary>
    /// <value>
    /// The player two.
    /// </value>
    public Player PlayerTwo { get; } = playerTwo ?? throw new ArgumentNullException(nameof(playerTwo));

    /// <summary>
    /// Gets the player one field.
    /// </summary>
    /// <value>
    /// The player one field.
    /// </value>
    public PlayingField? PlayerOneField { get; private set; }

    /// <summary>
    /// Gets the player two field.
    /// </summary>
    /// <value>
    /// The player two field.
    /// </value>
    public PlayingField? PlayerTwoField { get; private set; }

    /// <summary>
    /// Gets the next move player.
    /// </summary>
    /// <value>
    /// The next move player.
    /// </value>
    public Player NextMovePlayer { get; private set; } = playerOne ?? throw new ArgumentNullException(nameof(playerOne));

    /// <summary>
    /// Gets the winner.
    /// </summary>
    /// <value>
    /// The winner.
    /// </value>
    public Player? Winner { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the game already ended.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the game already ended; otherwise, <c>false</c>.
    /// </value>
    public bool GameEnded => this.Winner != null;

    /// <summary>
    /// Initializes the game.
    /// </summary>
    /// <param name="shipsToPlace">The ships to be placed on the playing fields.</param>
    /// <param name="playingFieldDimensions">The playing field dimensions.</param>
    public void InitializeGame(List<Ship> shipsToPlace, Dimensions playingFieldDimensions)
    {
        _ = playingFieldDimensions ?? throw new ArgumentNullException(nameof(playingFieldDimensions));
        _ = shipsToPlace ?? throw new ArgumentNullException(nameof(shipsToPlace));

        this.PlayerOneField = new PlayingField(playingFieldDimensions);

        this.PlayerOneField.RandomlyPlaceShips(shipsToPlace);

        this.PlayerTwoField = new PlayingField(playingFieldDimensions);

        this.PlayerTwoField.RandomlyPlaceShips(shipsToPlace);
    }

    /// <summary>
    /// Gets the playing field at which the next move player will be firing at.
    /// </summary>
    /// <returns>Playing field representation.</returns>
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

    /// <summary>
    /// Changes the next move player.
    /// </summary>
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

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void EndGame()
    {
        this.Winner = this.NextMovePlayer;
    }
}