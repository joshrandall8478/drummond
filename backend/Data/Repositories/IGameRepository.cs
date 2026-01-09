using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Data.Repositories
{
    public interface IGameRepository
    {
        Task InitializeAsync();
        Task<GameCriteria> GenerateCriteria();
        Task<List<PlayerStats>> GetMatchingPlayersAsync(GameCriteria criteria, List<string> excludedPositions);
        int CalculatePoints(PlayerStats player);
    }
}