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

        "1+ All-Star Selection",
        "3+ All-Star Selections",
        "5+ All-Star Selections",

        "25+ PPG Career",
        "20+ PPG Career",
        "15+ PPG Career",
        "10+ PPG Career",
        "5+ PPG Career",
        "Under 5 PPG Career",

        "10+ RPG Career",
        "8+ RPG Career",
        "5+ RPG Career",
        "3+ RPG Career",
        "Under 3 RPG Career",

        "8+ APG Career",
        "5+ APG Career",
        "3+ APG Career",
        "1+ APG Career",
        "Under 1 APG Career",

        "1.5+ SPG Career",
        "1+ SPG Career",
        "0.5+ SPG Career",

        "1.5+ BPG Career",
        "1+ BPG Career",
        "0.5+ BPG Career",

        "Lottery Pick",
        "Undrafted",

        "Drafted in 2010s",
        "Drafted in 2000s",
        "Drafted in 1990s",
        "Drafted Before 1990",
        "Drafted 2015 or Later",
        "Drafted 2010 or Earlier",

        "10+ Years in League",
        "5-9 Years in League",
        "0-4 Years in League",
        "Rookie (1 Year)",

        "Went to a College with State in its Name",
        "Went to a College with Michigan in its Name",

        "20+ PPG and 5+ APG",
        "10+ RPG and 1+ BPG",
        "5+ APG and 1+ SPG",
        "Champion Without All-Star"
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

            "1+ Championship" => player.Rings >= 1,
            "2+ Championships" => player.Rings >= 2,

            "1+ All-Star Selection" => player.AllStars >= 1,
            "3+ All-Star Selections" => player.AllStars >= 3,
            "5+ All-Star Selections" => player.AllStars >= 5,

            "25+ PPG Career" => player.Ppg >= 25,
            "20+ PPG Career" => player.Ppg >= 20,
            "15+ PPG Career" => player.Ppg >= 15,
            "10+ PPG Career" => player.Ppg >= 10,
            "5+ PPG Career" => player.Ppg >= 5,
            "Under 5 PPG Career" => player.Ppg < 5,

            "10+ RPG Career" => player.Rpg >= 10,
            "8+ RPG Career" => player.Rpg >= 8,
            "5+ RPG Career" => player.Rpg >= 5,
            "3+ RPG Career" => player.Rpg >= 3,
            "Under 3 RPG Career" => player.Rpg < 3,

            "8+ APG Career" => player.Apg >= 8,
            "5+ APG Career" => player.Apg >= 5,
            "3+ APG Career" => player.Apg >= 3,
            "1+ APG Career" => player.Apg >= 1,
            "Under 1 APG Career" => player.Apg < 1,

            "1.5+ SPG Career" => player.Spg >= 1.5m,
            "1+ SPG Career" => player.Spg >= 1,
            "0.5+ SPG Career" => player.Spg >= 0.5m,

            "1.5+ BPG Career" => player.Bpg >= 1.5m,
            "1+ BPG Career" => player.Bpg >= 1,
            "0.5+ BPG Career" => player.Bpg >= 0.5m,

            "Lottery Pick" => player.IsLottery == 1,
            "Undrafted" => player.DraftYear == -1,

            "Drafted in 2010s" => player.DraftYear >= 2010 && player.DraftYear <= 2019,
            "Drafted in 2000s" => player.DraftYear >= 2000 && player.DraftYear <= 2009,
            "Drafted in 1990s" => player.DraftYear >= 1990 && player.DraftYear <= 1999,
            "Drafted Before 1990" => player.DraftYear < 1990 && player.DraftYear != -1,
            "Drafted 2015 or Later" => player.DraftYear >= 2015,
            "Drafted 2010 or Earlier" => player.DraftYear <= 2010 && player.DraftYear != -1,

            "10+ Years in League" => player.YearsInLeague >= 10,
            "5-9 Years in League" => player.YearsInLeague >= 5 && player.YearsInLeague <= 9,
            "0-4 Years in League" => player.YearsInLeague >= 0 && player.YearsInLeague <= 4,
            "Rookie (1 Year)" => player.YearsInLeague == 1,

            "Went to a College with State in its Name" => !string.IsNullOrEmpty(player.College) && 
                                                           player.College.Contains("State", StringComparison.OrdinalIgnoreCase),
            "Went to a College with Michigan in its Name" => !string.IsNullOrEmpty(player.College) && 
                                                              player.College.Contains("Michigan", StringComparison.OrdinalIgnoreCase),
            "20+ PPG and 5+ APG" => player.Ppg >= 20 && player.Apg >= 5,
            "10+ RPG and 1+ BPG" => player.Rpg >= 10 && player.Bpg >= 1,
            "5+ APG and 1+ SPG" => player.Apg >= 5 && player.Spg >= 1,
            "Champion Without All-Star" => player.Rings > 0 && player.AllStars == 0,

            _ => false
        };
    }

    public int CalculatePoints(PlayerStats player)
    {
        int points = 500;

        return points;
    }
}