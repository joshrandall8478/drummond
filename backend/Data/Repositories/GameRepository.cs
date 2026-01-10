using backend.Models;

namespace backend.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly INbaPlayerRepository _playerRepository;
    private readonly Random _random = new();
    private List<string> _teamNames = new();

    private readonly List<string> _statCategories = new()
    {
        "All-Stars",
        "MVP Winners",
        "Championship Winners",
        "DPOY Winners",
        "Rookie of the Year Winners",
        "6th Man Award Winners",

        "1+ Championship",
        "2+ Championships",
        "3+ Championships",

        "1+ All-Star Selection",
        "3+ All-Star Selections",
        "5+ All-Star Selections",
        "10+ All-Star Selections",

        "25+ PPG Career",
        "20+ PPG Career",
        "15+ PPG Career",
        "10+ PPG Career",
        "5+ PPG Career",

        "10+ RPG Career",
        "8+ RPG Career",
        "5+ RPG Career",
        "3+ RPG Career",

        "8+ APG Career",
        "5+ APG Career",
        "3+ APG Career",

        "1.5+ SPG Career",
        "1+ SPG Career",

        "Multiple MVP Winner"
    };

    public GameRepository(INbaPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task InitializeAsync()
    {
        _teamNames = await _playerRepository.GetAllTeamNamesAsync();
    }

    public async Task<GameCriteria> GenerateCriteria()
    {
        if (_teamNames.Count == 0)
        {
            _teamNames = await _playerRepository.GetAllTeamNamesAsync();
        }

        var allCategories = new List<string>();
        allCategories.AddRange(_teamNames);
        allCategories.AddRange(_statCategories);

        // pick two different categories!!
        var category1 = allCategories[_random.Next(allCategories.Count)];
        allCategories.Remove(category1);
        var category2 = allCategories[_random.Next(allCategories.Count)];

        return new GameCriteria
        {
            Category1 = category1,
            Category2 = category2
        };
    }

    public bool MatchesCriteria(PlayerStats player, string criteria)
    {
        // Initialize team names if empty (lazy loading)
        if (_teamNames.Count == 0)
        {
            _teamNames = _playerRepository.GetAllTeamNamesAsync().GetAwaiter().GetResult();
        }
        // check if criteria is a team name
        if (_teamNames.Contains(criteria))
        {
            return player.Teams?.Any(t => t == criteria) ?? false;
        }

        // check stat-based criteria
        return criteria switch
        {
            "All-Stars" => player.AllStars > 0,
            "MVP Winners" => player.Mvps > 0,
            "Championship Winners" => player.Rings > 0,
            "DPOY Winners" => player.Dpoys > 0,
            "Rookie of the Year Winners" => player.RookieOfTheYear,
            "6th Man Award Winners" => player.SixManAwards > 0,
            "Multiple MVP Winner" => player.Mvps >= 2,

            "1+ Championship" => player.Rings >= 1,
            "2+ Championships" => player.Rings >= 2,
            "3+ Championships" => player.Rings >= 3,

            "1+ All-Star Selection" => player.AllStars >= 1,
            "3+ All-Star Selections" => player.AllStars >= 3,
            "5+ All-Star Selections" => player.AllStars >= 5,
            "10+ All-Star Selections" => player.AllStars >= 10,

            "25+ PPG Career" => player.Ppg >= 25,
            "20+ PPG Career" => player.Ppg >= 20,
            "15+ PPG Career" => player.Ppg >= 15,
            "10+ PPG Career" => player.Ppg >= 10,
            "5+ PPG Career" => player.Ppg >= 5,

            "10+ RPG Career" => player.Rpg >= 10,
            "8+ RPG Career" => player.Rpg >= 8,
            "5+ RPG Career" => player.Rpg >= 5,
            "3+ RPG Career" => player.Rpg >= 3,

            "8+ APG Career" => player.Apg >= 8,
            "5+ APG Career" => player.Apg >= 5,
            "3+ APG Career" => player.Apg >= 3,

            "1.5+ SPG Career" => player.Spg >= 1.5m,
            "1+ SPG Career" => player.Spg >= 1,

            _ => false
        };
    }

    public int CalculatePoints(PlayerStats player)
    {
        int points = 500;

        return points;
    }
}