using Microsoft.AspNetCore.Mvc;
using backend.Data.Repositories;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository _gameRepository;
    private readonly INbaPlayerRepository _playerRepository;

    public GameController(
        IGameRepository gameRepository,
        INbaPlayerRepository playerRepository)
    {
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
    }

    // POST: game/start
    // starts a new game and returns initial criteria
    [HttpPost("start")]
    public async Task<ActionResult> StartGame()
    {
        try
        {
            var criteria = await _gameRepository.GenerateCriteria();
            
            return Ok(new
            {
                criteria,
                score = 0,
                lineup = new GameLineup()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

[HttpPost("select-player")]
public async Task<ActionResult> SelectPlayer([FromBody] SelectPlayerRequest request)
{
    try
    {
        var player = request.Player;
        var criteria = request.Criteria;

        // extract the individual categories
        var category1 = criteria.Category1;
        var category2 = criteria.Category2;

        // verify position matches
        if (player.Position != request.Position)
        {
            return Ok(new
            {
                success = false,
                error = "Player position doesn't match selected position"
            });
        }

        var match1 = _gameRepository.MatchesCriteria(player, category1);
        
        var match2 = _gameRepository.MatchesCriteria(player, category2);

        if (!(match1 && match2))
        {
            return Ok(new
            {
                success = false,
                error = "This player does not match both categories!"
            });
        }

        var points = _gameRepository.CalculatePoints(player);

        // generate next criteria if game not complete
        GameCriteria? nextCriteria = null;
        if (!request.IsGameComplete)
        {
            nextCriteria = await _gameRepository.GenerateCriteria();
        }

        return Ok(new
        {
            success = true,
            points,
            player = new
            {
                player.PlayerId,
                player.FirstName,
                player.LastName,
                player.Position
            },
            nextCriteria
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message });
    }
}

// Request Models
public class GetPlayersRequest
{
    public GameCriteria Criteria { get; set; } = new();
    public List<string> FilledPositions { get; set; } = new();
}

public class SelectPlayerRequest
{
    public PlayerStats Player { get; set; } = new();
    public string Position { get; set; } = string.Empty;
    public bool IsGameComplete { get; set; }
    public GameCriteria Criteria { get; set; } = new();
    public List<string> FilledPositions { get; set; } = new();
}
}