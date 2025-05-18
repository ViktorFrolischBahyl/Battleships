using Battleships.Models;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BattleshipsController(ILogger<BattleshipsController> logger, IBattleshipsService battleshipsService)
    : ControllerBase
{
    private ILogger<BattleshipsController> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    private IBattleshipsService BattleshipsService { get; } = battleshipsService ?? throw new ArgumentNullException(nameof(battleshipsService));

    [HttpGet]
    [ActionName("health-check")]
    public ActionResult<string> Get()
    {
        this.Logger.LogDebug("health-check method initiated.");

        return this.Ok();
    }

    [HttpPost]
    [ActionName("start-game")]
    public ActionResult<CreateGameOutput> Post([FromBody] CreateGameInput createGameInput)
    {
        _ = createGameInput ?? throw new ArgumentNullException(nameof(createGameInput));

        this.Logger.LogDebug("start-game method initiated.");

        var createdGame = this.BattleshipsService.CreateGame(createGameInput);

        return this.Ok(new CreateGameOutput()
        {
            GameId = createdGame.GameId,
        });
    }
}