using System.Data.SqlTypes;
using Dapper;
using infrastructure.dataModels;
using Npgsql;

namespace infrastructure.repository;

public class PasswordHashRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PasswordHashRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public PasswordHash GetById(string email)
    {
        var sql = $@"
            select * from users.password_hash
            JOIN users.user ON password_hash.user_id = id
            WHERE email = @email;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<PasswordHash>(sql, new { email }) ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new SqlTypeException("Find User");
        }
    }
    
    public bool Create(PasswordHash passwordHash)
    {
        // Define the SQL query to insert a new password hash
        string sql = "INSERT INTO users.password_hash (user_id, hash, salt, algorithm) " + 
                     "VALUES (@Id, @Hash, @Salt, @Algorithm)";
        try
        { 
            // Create an object with the provided data
            using var conn = _dataSource.OpenConnection();
            // Execute the SQL query using Dapper
            conn.Execute(sql, passwordHash);
            return true;
        }
        catch
        {
            throw new SqlTypeException("Create User");
        }
    }

    public void Update(string userId, string hash, string salt, string algorithm)
    {
        throw new NotImplementedException();
    }
}