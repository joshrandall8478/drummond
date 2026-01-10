using MySqlConnector;

namespace backend.Data;

public class Database
{
    private readonly string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public async Task<MySqlConnection> GetOpenConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}