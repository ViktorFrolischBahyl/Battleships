using Battleships.Models.Game;
using System.Text.Json.Serialization;

namespace Battleships.Models;

public class FireOutput
{
    public string GameId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HitState HitState { get; set; }

    public bool GameEnded => this.Winner != null;

    public Player? Winner { get; set; } = null;
}