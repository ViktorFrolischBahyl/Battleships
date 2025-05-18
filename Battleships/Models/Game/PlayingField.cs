using Battleships.Models.Exceptions;
using System.ComponentModel;

namespace Battleships.Models.Game;

/// <summary>
/// Model representing the playing field of the game.
/// </summary>
public class PlayingField
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayingField"/> class.
    /// </summary>
    /// <param name="playingFieldDimensions">The playing field dimensions.</param>
    /// <exception cref="System.ArgumentNullException">playingFieldDimensions</exception>
    public PlayingField(Dimensions playingFieldDimensions)
    {
        this.PlayingFieldDimensions = playingFieldDimensions ?? throw new ArgumentNullException(nameof(playingFieldDimensions));

        this.Grid = new Cell[playingFieldDimensions.X, playingFieldDimensions.Y];

        for (var x = 0; x < this.PlayingFieldDimensions.X; x++)
        {
            for (var y = 0; y < this.PlayingFieldDimensions.Y; y++)
            {
                this.Grid[x, y] = new Cell()
                {
                    State = CellState.Water,
                    X = x,
                    Y = y,
                };
            }
        }
    }

    /// <summary>
    /// Gets the playing field dimensions.
    /// </summary>
    /// <value>
    /// The playing field dimensions.
    /// </value>
    public Dimensions PlayingFieldDimensions { get; }

    /// <summary>
    /// Gets the grid.
    /// </summary>
    /// <value>
    /// The grid.
    /// </value>
    public Cell[,] Grid { get; }

    /// <summary>
    /// Gets the fleet.
    /// </summary>
    /// <value>
    /// The fleet.
    /// </value>
    public List<Ship> Fleet { get; } = [];

    /// <summary>
    /// Randomly place the ships.
    /// </summary>
    /// <param name="shipsToPlace">The ships to place.</param>
    public void RandomlyPlaceShips(List<Ship> shipsToPlace)
    {
        var random = new Random();

        shipsToPlace.ForEach(shipToPlace =>
        {
            var possiblePositions = this.GetPossiblePositions(shipToPlace);

            if (possiblePositions.Count == 0)
            {
                throw new InvalidOperationException($"Unable to fit all ships to the playing field with dimensions {this.PlayingFieldDimensions.X}x{this.PlayingFieldDimensions.Y}");
            }

            var randomPositionIndex = random.Next(possiblePositions.Count);

            var randomPosition = possiblePositions.ElementAt(randomPositionIndex);

            randomPosition.ForEach(cell => cell.State = CellState.Ship);

            var shipToAdd = new Ship()
            {
                Length = shipToPlace.Length,
                Type = shipToPlace.Type,
            };

            shipToAdd.Position.AddRange(randomPosition);

            this.Fleet.Add(shipToAdd);
        });
    }

    /// <summary>
    /// Gets the string representation of grid.
    /// </summary>
    /// <returns>String representing the grid.</returns>
    public string GetStringRepresentationOfGrid()
    {
        var gridRepresentation = this.GetStringRepresentationOfGrid(water: ' ', ship: 'O', hit: 'X', miss: '-');

        return gridRepresentation;
    }

    /// <summary>
    /// Gets the string representation of grid with hidden ships.
    /// </summary>
    /// <returns>String representing the grid with hidden ships.</returns>
    public string GetStringRepresentationOfGridWithHiddenShips()
    {
        var gridRepresentation = this.GetStringRepresentationOfGrid(water: ' ', ship: ' ', hit: 'X', miss: '-');

        return gridRepresentation;
    }

    /// <summary>
    /// Fires at the specified cell dimensions.
    /// </summary>
    /// <param name="cellDimensions">The cell dimensions.</param>
    /// <returns>Representation of the cell fired at.</returns>
    public Cell Fire(Dimensions cellDimensions)
    {
        _ = cellDimensions ?? throw new ArgumentNullException(nameof(cellDimensions));

        if (cellDimensions.X < 0 || cellDimensions.X >= this.PlayingFieldDimensions.X
            || cellDimensions.Y < 0 || cellDimensions.Y >= this.PlayingFieldDimensions.Y)
        {
            throw new OutsideOfDefinedPlayingField($"Cell (X:{cellDimensions.X}, Y:{cellDimensions.Y}) is outside of defined playing field!");
        }

        var cell = this.Grid[cellDimensions.X, cellDimensions.Y];

        switch (cell.State)
        {
            case CellState.Water:
                cell.State = CellState.Miss;
                return cell;
            case CellState.Ship:
                cell.State = CellState.Hit;
                return cell;
            case CellState.Hit:
            case CellState.Miss:
                throw new AlreadyFiredAtException($"Cell (X:{cellDimensions.X}, Y:{cellDimensions.Y}) has already been fired at!");
            default:
                throw new InvalidEnumArgumentException(nameof(Cell.State), (int)cell.State, typeof(CellState));
        }
    }

    /// <summary>
    /// Indicates whether the cell is part of a ship that is completely sunk.
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <returns><c>true</c> if ship corresponding to the cell is completely sunk; otherwise, <c>false</c>.</returns>
    public bool WasWholeShipDestroyed(Cell cell)
    {
        _ = cell ?? throw new ArgumentNullException(nameof(cell));

        if (cell.State != CellState.Hit)
        {
            return false;
        }

        var ship = this.Fleet.FindAll(ship => ship.Position.Contains(cell)).Single();

        return this.WasWholeShipDestroyed(ship);
    }

    /// <summary>
    /// Indicates whether all the ships in the fleet have been sunk.
    /// </summary>
    /// <returns><c>true</c> if all ships in the fleet have been sunk; otherwise, <c>false</c>.</returns>
    public bool AreAllShipsDestroyed()
    {
        return this.Fleet.All(this.WasWholeShipDestroyed);
    }

    /// <summary>
    /// Indicates whether the ship is completely sunk.
    /// </summary>
    /// <param name="ship">The ship.</param>
    /// <returns>
    /// True if the ship is completely sunk; false otherwise.
    /// </returns>
    private bool WasWholeShipDestroyed(Ship ship)
    {
        _ = ship ?? throw new ArgumentNullException(nameof(ship));

        return ship.Position.All(cell => cell.State == CellState.Hit);
    }

    /// <summary>
    /// Gets all the possible positions for the ship.
    /// </summary>
    /// <param name="ship">The ship.</param>
    /// <returns>Lit of all possible positions for the provided ship.</returns>
    private List<List<Cell>> GetPossiblePositions(Ship ship)
    {
        var positions = new List<List<Cell>>();

        var length = ship.Length;

        for (var x = 0; x < this.PlayingFieldDimensions.X; x++)
        {
            for (var y = 0; y < this.PlayingFieldDimensions.Y; y++)
            {
                var horizontalPosition = new List<Cell>();

                for (var i = 0; i < length; i++)
                {
                    if (x + i >= this.PlayingFieldDimensions.X)
                    {
                        break;
                    }

                    horizontalPosition.Add(this.Grid[x + i, y]);
                }

                if (horizontalPosition.Count == length && this.IsPositionValid(horizontalPosition))
                {
                    positions.Add(horizontalPosition);
                }

                if (length == 1)
                {
                    continue;
                }

                var verticalPosition = new List<Cell>();

                for (var i = 0; i < length; i++)
                {
                    if (y + i >= this.PlayingFieldDimensions.Y)
                    {
                        break;
                    }

                    verticalPosition.Add(this.Grid[x, y + i]);
                }

                if (verticalPosition.Count == length && this.IsPositionValid(verticalPosition))
                {
                    positions.Add(verticalPosition);
                }
            }
        }

        return positions;
    }

    /// <summary>
    /// Determines whether the specified position is valid in Battleships game.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    ///   <c>true</c> if the specified position is valid in Battleships game; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">position</exception>
    private bool IsPositionValid(List<Cell> position)
    {
        _ = position ?? throw new ArgumentNullException(nameof(position));

        foreach (var cell in position)
        {
            if (this.Grid[cell.X, cell.Y].State != CellState.Water)
            {
                return false;
            }

            if (!this.AdjacentCellsAreWater(cell))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the adjacent cells are all water.
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <returns>
    ///   <c>true</c> if the adjacent cells are all water; otherwise, <c>false</c>.
    /// </returns>
    private bool AdjacentCellsAreWater(Cell cell)
    {
        _ = cell ?? throw new ArgumentNullException(nameof(cell));

        var x = cell.X;
        var y = cell.Y;

        if (x - 1 >= 0
            && this.Grid[x - 1, y].State != CellState.Water)
        {
            return false;
        }

        if (y - 1 >= 0
            && this.Grid[x, y - 1].State != CellState.Water)
        {
            return false;
        }

        if (x + 1 < this.PlayingFieldDimensions.X
            && this.Grid[x + 1, y].State != CellState.Water)
        {
            return false;
        }

        if (y + 1 < this.PlayingFieldDimensions.Y
            && this.Grid[x, y + 1].State != CellState.Water)
        {
            return false;
        }

        if (x - 1 >= 0
            && y - 1 >= 0
            && this.Grid[x - 1, y - 1].State != CellState.Water)
        {
            return false;
        }

        if (x + 1 < this.PlayingFieldDimensions.X
            && y + 1 < this.PlayingFieldDimensions.Y
            && this.Grid[x + 1, y + 1].State != CellState.Water)
        {
            return false;
        }

        if (x - 1 >= 0
            && y + 1 < this.PlayingFieldDimensions.Y
            && this.Grid[x - 1, y + 1].State != CellState.Water)
        {
            return false;
        }

        if (x + 1 < this.PlayingFieldDimensions.X
            && y - 1 >= 0
            && this.Grid[x + 1, y - 1].State != CellState.Water)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the string representation of the grid, with specified characters for all the states.
    /// </summary>
    /// <param name="water">The water.</param>
    /// <param name="ship">The ship.</param>
    /// <param name="hit">The hit.</param>
    /// <param name="miss">The miss.</param>
    /// <returns>String representing the grid.</returns>
    private string GetStringRepresentationOfGrid(char water, char ship, char hit, char miss)
    {
        var gridRepresentation = string.Empty;

        for (var y = 0; y < this.PlayingFieldDimensions.Y; y++)
        {
            var row = string.Empty;

            for (var x = 0; x < this.PlayingFieldDimensions.X; x++)
            {
                if (x == 0)
                {
                    row += '|';
                }

                row += this.Grid[x, y].State switch
                {
                    CellState.Water => water,
                    CellState.Ship => ship,
                    CellState.Hit => hit,
                    CellState.Miss => miss,
                    _ => throw new InvalidEnumArgumentException(nameof(Cell.State), (int)this.Grid[x, y].State, typeof(CellState)),
                };

                row += '|';
            }

            gridRepresentation += row + Environment.NewLine;
        }

        return gridRepresentation;
    }
}