using Battleships.Models.Game;
using Battleships.Models.Settings;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Battleships.Services;

/// <summary>
/// Implementation of the <see cref="ILoadShipsDefinitionsService" /> interface for loading ship definitions from a file.
/// </summary>
/// <seealso cref="Battleships.Services.Interfaces.ILoadShipsDefinitionsService" />
public class LoadShipsDefinitionsFromFileService(
    ILogger<BattleshipsService> logger,
    IOptions<ApplicationSettings> settings)
    : ILoadShipsDefinitionsService
{
    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <value>
    /// The settings.
    /// </value>
    private ApplicationSettings Settings { get; } = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    private ILogger<BattleshipsService> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public List<Ship> LoadShipsDefinitions()
    {
        var path = Path.GetFullPath(this.Settings.PathToShipsDefinitionFile);

        var file = File.ReadAllLines(path);

        var ships = new List<List<string>>();

        var shipLines = new List<string>();

        foreach (var line in file)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (shipLines.Count != 0)
                {
                    ships.Add(shipLines);
                }
                shipLines = [];
            }
            else
            {
                shipLines.Add(line);
            }
        }

        if (shipLines.Count != 0)
        {
            ships.Add(shipLines);
        }

        var result = new List<Ship>();

        ships.ForEach(shipAsStrings =>
        {
            var type = shipAsStrings.First();

            shipAsStrings.RemoveAt(0);
            var max = shipAsStrings.Max(line => line.Length);
            
            var shape = new Cell[max, shipAsStrings.Count];

            for (int x = 0; x < max; x++)
            {
                for (int y = 0; y < shipAsStrings.Count; y++)
                {
                    var line = shipAsStrings[y];

                    if (x < line.Length)
                    {
                        shape[x, y] = new Cell()
                        {
                            State = line[x] is 'X' or 'x' ? CellState.Ship : CellState.Water,
                        };
                    }
                    else
                    {
                        shape[x, y] = new Cell()
                        {
                            State = CellState.Water,
                        };
                    }
                }
            }

            var ship = new Ship(shape)
            {
                Type = type,
            };

            result.Add(ship);
        });

        return result;
    }
}