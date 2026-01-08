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

    [HttpGet]
    public async Task<ActionResult<List<Player>>> GetAllPlayers()
    {
        try
        {
            var players = await _playerRepository.GetAllPlayersAsync();
            return Ok(players);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPlayerWithStats(int id)
    {
       return null;
    }
}