// Controllers/PlayersController.cs
using Microsoft.AspNetCore.Mvc;
using backend.Data.Repositories;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly INbaPlayerRepository _playerRepository;

    public PlayersController(INbaPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    // GET: /players
    [HttpGet]
    public async Task<ActionResult<List<Player>>> GetAllPlayers()
    {
        try
        {
            var players = await _playerRepository.GetAllPlayersWithStatsAsync();
            return Ok(players);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // GET: api/players/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult> GetPlayerById(int id)
    {
        try
        {
            var playerStats = await _playerRepository.GetPlayerStatsAsync(id);
            
            if (playerStats == null)
                return NotFound(new { error = "Player not found" });

            return Ok(playerStats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}