using MySqlConnector;
using Dapper;
using backend.Models;

namespace backend.Data.Repositories;

public class NbaPlayerRepository : INbaPlayerRepository
{
    private readonly Database _database;

    public NbaPlayerRepository(Database database)
    {
        _database = database;
    }

    public async Task<List<string>> GetAllTeamNamesAsync()
    {
        var sql = @"
        SELECT team_name
        FROM nba_teams
        WHERE team_id NOT IN (
            1610610034,
            1610610031,
            1610610028,
            1610610025,
            1610610030,
            1610610036,
            1610610026,
            1610610032,
            1610610035,
            1610610029,
            1610610023,
            1610610037,
            1610610033
        )
        ORDER BY team_name;
    ";

        using var connection = _database.GetConnection();
        var teams = await connection.QueryAsync<string>(sql);
        return teams.ToList();
    }

    public async Task<PlayerStats> GetPlayerStatsAsync(int playerId)
    {
        var sql = @"
            SELECT 
                p.first_name as FirstName,
                p.last_name as LastName,
                p.position as Position,
                s.player_id as PlayerId,
                s.ppg as Ppg,
                s.rpg as Rpg,
                s.apg as Apg,
                s.spg as Spg,
                s.bpg as Bpg,
                s.college as College,
                s.draft_year as DraftYear,
                s.is_lottery as IsLottery, 
                s.years_in_league as YearsInLeague,
                s.all_stars as AllStars,
                s.mvps as Mvps,
                s.dpoys as Dpoys,
                s.six_man_awards as SixManAwards,
                s.rings as Rings,
                s.rookie_of_the_year as RookieOfTheYear
            FROM nba_player_stats s
            JOIN nba_players p ON s.player_id = p.player_id
            WHERE s.player_id = @PlayerId";
        
        using var connection = _database.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<PlayerStats>(sql, new { PlayerId = playerId });
    }

    public async Task<List<PlayerStats>> GetAllPlayersWithStatsAsync()
    {
       var sql = @"
            SELECT 
                p.player_id as PlayerId,
                p.first_name as FirstName,
                p.last_name as LastName,
                p.position as Position,
                COALESCE(s.ppg, 0) as Ppg,
                COALESCE(s.rpg, 0) as Rpg,
                COALESCE(s.apg, 0) as Apg,
                COALESCE(s.spg, 0) as Spg,
                COALESCE(s.bpg, 0) as Bpg,
                s.college as College,
                s.draft_year as DraftYear,
                s.is_lottery as IsLottery,
                s.years_in_league as YearsInLeague,
                COALESCE(s.all_stars, 0) as AllStars,
                COALESCE(s.mvps, 0) as Mvps,
                COALESCE(s.dpoys, 0) as Dpoys,
                COALESCE(s.six_man_awards, 0) as SixManAwards,
                COALESCE(s.rings, 0) as Rings,
                COALESCE(s.rookie_of_the_year, FALSE) as RookieOfTheYear,
                t.team_name as TeamName
            FROM nba_players p
            LEFT JOIN nba_player_stats s ON p.player_id = s.player_id
            LEFT JOIN player_teams pt ON p.player_id = pt.player_id
            LEFT JOIN nba_teams t ON pt.team_id = t.team_id
            ORDER BY p.player_id";
        
        using var connection = _database.GetConnection();
        
        var playerDict = new Dictionary<int, PlayerStats>();
        
        await connection.QueryAsync<PlayerStats, string, PlayerStats>(
            sql,
            (player, teamName) =>
            {
                if (!playerDict.TryGetValue(player.PlayerId, out var existingPlayer))
                {
                    existingPlayer = player;
                    playerDict.Add(player.PlayerId, existingPlayer);
                }
                
                if (!string.IsNullOrEmpty(teamName) && !existingPlayer.Teams.Contains(teamName))
                {
                    existingPlayer.Teams.Add(teamName);
                }
                
                return existingPlayer;
            },
            splitOn: "TeamName"
        );
        
        return playerDict.Values.ToList();
    }
}