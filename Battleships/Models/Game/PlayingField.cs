namespace Battleships.Models.Game;

public class PlayingField
{
    public PlayingField(Dimensions playingFieldDimensions)
    {
        PlayingFieldDimensions = playingFieldDimensions ?? throw new ArgumentNullException(nameof(playingFieldDimensions));

        Grid = new Cell[playingFieldDimensions.X, playingFieldDimensions.Y];

        for (int x = 0; x < PlayingFieldDimensions.X; x++)
        {
            for (int y = 0; y < PlayingFieldDimensions.Y; y++)
            {
                Grid[x, y] = new Cell()
                {
                    State = CellState.Water,
                    X = x,
                    Y = y,
                };
            }
        }
    }

    public string GetStringRepresentationOfGrid()
    {
        string gridRepresentation = string.Empty;

        for (int y = 0; y < PlayingFieldDimensions.Y; y++)
        {
            var row = string.Empty;

            for (int x = 0; x < PlayingFieldDimensions.X; x++)
            {
                row += Grid[x, y].State switch
                {
                    CellState.Water => " ",
                    CellState.Ship => "O",
                    CellState.Hit => "X",
                    CellState.Miss => "-",
                    _ => throw new ArgumentOutOfRangeException(nameof(Cell.State), $"Unknown cell state: {Grid[x, y].State}"),
                };

                row += "|";
            }

            gridRepresentation += row + Environment.NewLine;
        }

        return gridRepresentation;
    }

    private Dimensions PlayingFieldDimensions { get; }

    private Cell[,] Grid { get; }

    private List<Ship> Fleet { get; } = new List<Ship>();

    public void RandomizeShips(List<Ship> shipsToRandomize)
    {
        var random = new Random();

        shipsToRandomize.ForEach(shipToPlace =>
        {
            var possiblePositions = GetPossiblePositions(shipToPlace);

            if (possiblePositions.Count == 0)
            {
                throw new InvalidOperationException($"Unable to fit all ships to the playing field with dimensions {PlayingFieldDimensions.X}x{PlayingFieldDimensions.Y}");
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

            Fleet.Add(shipToAdd);
        });
    }

    private List<List<Cell>> GetPossiblePositions(Ship ship)
    {
        var positions = new List<List<Cell>>();

        var length = ship.Length;

        for (int x = 0; x < PlayingFieldDimensions.X; x++)
        {
            for (int y = 0; y < PlayingFieldDimensions.Y; y++)
            {
                var horizontalPosition = new List<Cell>();

                for (int i = 0; i < length; i++)
                {
                    if (x + i >= PlayingFieldDimensions.X)
                    {
                        break;
                    }

                    horizontalPosition.Add(Grid[x + i, y]);
                }

                if (horizontalPosition.Count == length && IsPositionValid(horizontalPosition))
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
                    if (y + i >= PlayingFieldDimensions.Y)
                    {
                        break;
                    }

                    verticalPosition.Add(Grid[x, y + i]);
                }

                if (verticalPosition.Count == length && IsPositionValid(verticalPosition))
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
            if (Grid[cell.X, cell.Y].State != CellState.Water)
            {
                return false;
            }

            if (!AdjacentCellsAreWater(cell))
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
            && Grid[x - 1, y].State != CellState.Water)
        {
            return false;
        }

        if (y - 1 >= 0
            && Grid[x, y - 1].State != CellState.Water)
        {
            return false;
        }

        if (x - 1 >= 0
            && y - 1 >= 0
            && Grid[x - 1, y - 1].State != CellState.Water)
        {
            return false;
        }

        if (x + 1 < PlayingFieldDimensions.X
            && Grid[x + 1, y].State != CellState.Water)
        {
            return false;
        }

        if (y + 1 < PlayingFieldDimensions.Y
            && Grid[x, y + 1].State != CellState.Water)
        {
            return false;
        }

        if (x + 1 < PlayingFieldDimensions.X
            && y + 1 < PlayingFieldDimensions.Y
            && Grid[x + 1, y + 1].State != CellState.Water)
        {
            return false;
        }

        return true;
    }
}