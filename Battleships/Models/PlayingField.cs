namespace Battleships.Models;

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

    private Dimensions PlayingFieldDimensions { get; }

    private Cell[,] Grid { get; }

    private List<Ship> Fleet { get; } = new List<Ship>();

    public void RandomizeShips(List<Ship> shipsToRandomize)
    {
        var random = new Random();

        shipsToRandomize.ForEach(shipToPlace =>
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

    private List<List<Cell>> GetPossiblePositions(Ship ship)
    {
        // TODO > Add logic for finding possible positions
        var positions = new List<List<Cell>>();

        var length = ship.Length;

        for (int x = 0; x < this.PlayingFieldDimensions.X; x++)
        {
            for (int y = 0; y < this.PlayingFieldDimensions.Y; y++)
            {
                if (this.Grid[x,y].State is CellState.Water)
                {
                    if (x - 1 >= 0)
                    {

                    }

                    if (y - 1 >= 0)
                    {

                    }

                    var position = new List<Cell>();
                }
            }
        }

        return positions;
    }
}