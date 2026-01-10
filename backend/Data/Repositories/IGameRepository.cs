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
        bool MatchesCriteria(PlayerStats player, string criteria); 
        int CalculatePoints(PlayerStats player);
    }
}