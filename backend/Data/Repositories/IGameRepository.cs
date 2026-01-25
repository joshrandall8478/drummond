using backend.Models;

namespace backend.Data.Repositories;

public interface IGameRepository
{
    Task InitializeAsync();
    Task<DailyGame> GetOrCreateTodayGameAsync(string seed);
    bool MatchesCriteria(PlayerStats player, string criteria);
    int CalculatePoints(PlayerStats player);
}