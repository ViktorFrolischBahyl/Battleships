using System.ComponentModel.DataAnnotations;
using Battleships.Models;
using Battleships.Models.Exceptions;
using Battleships.Models.Game;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[ApiController]
[Route("api/[controller]/[action]")]
public class BattleshipsController(ILogger<BattleshipsController> logger, IBattleshipsService battleshipsService)
    : ControllerBase
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    private ILogger<BattleshipsController> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Gets the battleships service.
    /// </summary>
    /// <value>
    /// The battleships service.
    /// </value>
    private IBattleshipsService BattleshipsService { get; } = battleshipsService ?? throw new ArgumentNullException(nameof(battleshipsService));

    /// <summary>
    /// Checks whether Battleships controller is available.
    /// </summary>
    /// <returns>Response to the request.</returns>
    /// <response code="200">Battleships controller ready.</response>
    /// <response code="500">Battleships controller not ready.</response>
    [HttpGet]
    [ActionName("health-check")]
    public ActionResult<string> Get()
    {
        this.Logger.LogDebug("health-check method initiated.");

        return this.Ok();
    }

    /// <summary>
    /// Starts new game.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// Game created specification.
    /// </returns>
    /// <response code="200">Game started.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Unrecognized internal server error.</response>
    [HttpPost]
    [ActionName("start-game")]
    public ActionResult<CreateGameOutput> Post([FromBody] CreateGameInput input)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        try
        {
            if (input.PlayerOne.Name == input.PlayerTwo.Name)
            {
                return this.BadRequest($"Player One and Two cannot have same name!");
            }

            this.Logger.LogDebug("start-game method initiated.");

            var createdGame = this.BattleshipsService.CreateGame(input);

            this.Logger.LogDebug("start-game method finished.");

            return this.Ok(new CreateGameOutput()
            {
                GameId = createdGame.GameId,
            });
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "start-game method finished with errors.");
            throw;
        }
    }

    /// <summary>
    /// Sends fire action to the specified game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    /// <param name="cellDimensions">The cell dimensions.</param>
    /// <returns>Information about the game state after the fire action.</returns>
    /// <response code="200">Game updated.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Game not found.</response>
    /// <response code="500">Unrecognized internal server error.</response>
    [HttpPut]
    [ActionName("next-move/{gameId}")]
    public ActionResult<FireOutput> Put(
        [FromRoute]
        [Required]
        string gameId,
        [FromBody]
        [Required]
        Dimensions cellDimensions)
    {
        _ = cellDimensions ?? throw new ArgumentNullException(nameof(cellDimensions));
        _ = gameId ?? throw new ArgumentNullException(nameof(gameId));

        try
        {
            this.Logger.LogDebug("next-move method initiated.");

            var input = new FireInput(gameId, cellDimensions);

            var result = this.BattleshipsService.Fire(input);

            this.Logger.LogDebug("next-move method finished.");

            return this.Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            this.Logger.LogError(ex, "next-move method finished with errors.");
            return this.NotFound(ex.Message);
        }
        catch (AlreadyFiredAtException ex)
        {
            this.Logger.LogError(ex, "next-move method finished with errors.");
            return this.BadRequest(ex.Message);
        }
        catch (OutsideOfDefinedPlayingField ex)
        {
            this.Logger.LogError(ex, "next-move method finished with errors.");
            return this.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "next-move method finished with errors.");
            throw;
        }
    }
}