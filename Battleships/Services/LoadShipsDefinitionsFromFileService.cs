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
    ILogger<LoadShipsDefinitionsFromFileService> logger,
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
    private ILogger<LoadShipsDefinitionsFromFileService> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public List<Ship> LoadShipsDefinitions()
    {
        this.Logger.LogDebug($"Method {nameof(this.LoadShipsDefinitions)} started.");

        var result = new List<Ship>();

        var path = Path.GetFullPath(this.Settings.PathToShipsDefinitionFile);

        var fileLines = File.ReadAllLines(path);

        var ships = this.GetShipsLineGrouped(fileLines);

        ships.ForEach(shipAsStrings =>
        {
            var type = shipAsStrings.First();

            shipAsStrings.RemoveAt(0);

            var shape = this.ParseShape(shipAsStrings);

            var ship = new Ship(shape)
            {
                Type = type,
            };

            result.Add(ship);
        });

        this.Logger.LogDebug($"Method {nameof(this.LoadShipsDefinitions)} ended with number of ships loaded: {result.Count}.");

        return result;
    }

    /// <summary>
    /// Gets the ships line grouped.
    /// </summary>
    /// <param name="fileLines">The file lines.</param>
    /// <returns>List of grouped lines by ships.</returns>
    private List<List<string>> GetShipsLineGrouped(string[] fileLines)
    {
        _ = fileLines ?? throw new ArgumentNullException(nameof(fileLines));

        this.Logger.LogDebug($"Method {nameof(this.GetShipsLineGrouped)} started.");

        var ships = new List<List<string>>();

        var shipLines = new List<string>();

        foreach (var line in fileLines)
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

        this.Logger.LogDebug($"Method {nameof(this.GetShipsLineGrouped)} ended.");

        return ships;
    }

    /// <summary>
    /// Parses the shape.
    /// </summary>
    /// <param name="shipAsStrings">The ship as strings.</param>
    /// <returns>Parsed shape.</returns>
    private Cell[,] ParseShape(List<string> shipAsStrings)
    {
        _ = shipAsStrings ?? throw new ArgumentNullException(nameof(shipAsStrings));

        this.Logger.LogDebug($"Method {nameof(this.ParseShape)} started.");

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

        this.Logger.LogDebug($"Method {nameof(this.ParseShape)} ended.");

        return shape;
    }
}