using Battleships.Models;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BattleshipsController : ControllerBase
{
    private readonly ILogger<BattleshipsController> logger;

    public BattleshipsController(ILogger<BattleshipsController> logger, IBattleshipsService battleshipsService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.BattleshipsService = battleshipsService ?? throw new ArgumentNullException(nameof(battleshipsService));
    }

    private IBattleshipsService BattleshipsService { get; }

    [HttpGet]
    [ActionName("health-check")]
    public ActionResult<string> Get()
    {
        this.logger.LogDebug("health-check method initiated.");

        return this.Ok();
    }

    [HttpPost]
    [ActionName("start-game")]
    public ActionResult<string> Post([FromBody] CreateGameInput createGameInput)
    {
        _ = createGameInput ?? throw new ArgumentNullException(nameof(createGameInput));

        this.logger.LogDebug("start-game method initiated.");

        var createdGame = this.BattleshipsService.CreateGame(createGameInput);

        return this.Ok(createdGame.GameId);
    }
}