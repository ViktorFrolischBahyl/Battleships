using System.ComponentModel;

namespace Battleships.Models.Game;

public class PlayingField
{
    public PlayingField(Dimensions playingFieldDimensions)
    {
        this.PlayingFieldDimensions = playingFieldDimensions ?? throw new ArgumentNullException(nameof(playingFieldDimensions));

        this.Grid = new Cell[playingFieldDimensions.X, playingFieldDimensions.Y];

        for (int x = 0; x < this.PlayingFieldDimensions.X; x++)
        {
            for (int y = 0; y < this.PlayingFieldDimensions.Y; y++)
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

    public Dimensions PlayingFieldDimensions { get; }

    public Cell[,] Grid { get; }

    public List<Ship> Fleet { get; } = [];

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

    public string GetStringRepresentationOfGrid()
    {
        string gridRepresentation = string.Empty;

        for (int y = 0; y < this.PlayingFieldDimensions.Y; y++)
        {
            var row = string.Empty;

            for (int x = 0; x < this.PlayingFieldDimensions.X; x++)
            {
                if (x == 0)
                {
                    row += "|";
                }

                row += this.Grid[x, y].State switch
                {
                    CellState.Water => " ",
                    CellState.Ship => "O",
                    CellState.Hit => "X",
                    CellState.Miss => "-",
                    _ => throw new InvalidEnumArgumentException(nameof(Cell.State), (int)this.Grid[x, y].State, typeof(CellState)),
                };

                row += "|";
            }

            gridRepresentation += row + Environment.NewLine;
        }

        return gridRepresentation;
    }

    public Cell Fire(Dimensions cellDimensions)
    {
        _ = cellDimensions ?? throw new ArgumentNullException(nameof(cellDimensions));

        if (cellDimensions.X < 0 || cellDimensions.X >= this.PlayingFieldDimensions.X
            || cellDimensions.Y < 0 || cellDimensions.Y >= this.PlayingFieldDimensions.Y)
        {
            throw new InvalidOperationException($"Dimension (X:{cellDimensions.X}, Y:{cellDimensions.Y}) are outside of defined playing field!");
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
                throw new InvalidOperationException($"Cell (X:{cellDimensions.X}, Y:{cellDimensions.Y}) has already been fired at!");
            default:
                throw new InvalidEnumArgumentException(nameof(Cell.State), (int)cell.State, typeof(CellState));
        }
    }

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

    public bool AreAllShipsDestroyed()
    {
        return this.Fleet.All(this.WasWholeShipDestroyed);
    }

    private bool WasWholeShipDestroyed(Ship ship)
    {
        _ = ship ?? throw new ArgumentNullException(nameof(ship));

        return ship.Position.All(cell => cell.State == CellState.Hit);
    }

    private List<List<Cell>> GetPossiblePositions(Ship ship)
    {
        var positions = new List<List<Cell>>();

        var length = ship.Length;

        for (int x = 0; x < this.PlayingFieldDimensions.X; x++)
        {
            for (int y = 0; y < this.PlayingFieldDimensions.Y; y++)
            {
                var horizontalPosition = new List<Cell>();

                for (int i = 0; i < length; i++)
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

                for (int i = 0; i < length; i++)
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
}