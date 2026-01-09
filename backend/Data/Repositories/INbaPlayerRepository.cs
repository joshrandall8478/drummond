using backend.Models;

namespace backend.Data.Repositories;

public interface INbaPlayerRepository
{
    Task<PlayerStats> GetPlayerStatsAsync(int playerId);
    Task<List<PlayerStats>> GetAllPlayersWithStatsAsync();
    Task<List<string>> GetAllTeamNamesAsync();
}