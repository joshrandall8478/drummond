using backend.Models;

namespace backend.Data.Repositories;

public interface INbaPlayerRepository
{
    Task<List<Player>> GetAllPlayersAsync();
    Task<Player?> GetPlayerByIdAsync(int playerId);
}