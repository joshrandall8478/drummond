using MySqlConnector;
using backend.Models;

namespace backend.Data.Repositories;

public class NbaPlayerRepository : INbaPlayerRepository
{
    private readonly Database _database;

    public NbaPlayerRepository(Database database)
    {
        _database = database;
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        var players = new List<Player>();
        
        using var conn = await _database.GetOpenConnectionAsync();
        using var cmd = new MySqlCommand(
            "SELECT player_id, first_name, last_name, position FROM nba_players", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            players.Add(new Player
            {
                PlayerId = reader.GetInt32("player_id"),
                FirstName = reader.GetString("first_name"),
                LastName = reader.GetString("last_name"),
                Position = reader.GetString("position")
            });
        }
        
        return players;
    }

    public async Task<Player?> GetPlayerByIdAsync(int playerId)
    {
        return null;
    }

}