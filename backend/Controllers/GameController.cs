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

    // POST: game/select-player
    // selects a player for a position, validates criteria, calculates points, generates next criteria
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
                return BadRequest(new { error = "Player position doesn't match selected position" });

            if(!(_gameRepository.MatchesCriteria(player, category1) && _gameRepository.MatchesCriteria(player, category2)))
            {
                return BadRequest(new { error = "This player does not match both categories!" });
            }

            // validate player matches criteria
            // var matchesCriteria = await _gameRepository.ValidatePlayerMatchesCriteria(
            //     player,
            //     request.Criteria
            // );

            // if (!matchesCriteria)
            //     return BadRequest(new { error = "This player does not match both categories!" });

            // Calculate points
            var points = _gameRepository.CalculatePoints(player);

            // Generate next criteria if game not complete
            GameCriteria? nextCriteria = null;
            if (!request.IsGameComplete)
            {
                nextCriteria = await _gameRepository.GenerateCriteria();
            }

            return Ok(new
            {
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