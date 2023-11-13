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

    public PasswordHash GetByEmail(string email)
    {
        var sql = $@"
            select * from users.password_hash
            where user_email = @email;
            ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<PasswordHash>(sql, new { email });
        }
        
    }
    

    public bool Create(string userId, string hash, string salt, string algorithm)
    {
        // Define the SQL query to insert a new password hash
        string sql = "INSERT INTO users.password_hash (user_email, hash, salt, algorithm) " + 
                     "VALUES (@userId, @hash, @salt, @algorithm)";

            // Create an object with the provided data
        var passwordHash = new
        { 
            userId = userId, 
            hash = hash, 
            salt = salt, 
            algorithm = algorithm
            };

        using (var conn = _dataSource.OpenConnection())
        {
            // Execute the SQL query using Dapper
            conn.Execute(sql, passwordHash);
            return true;
        }
    }

    public void Update(string userId, string hash, string salt, string algorithm)
    {
        throw new NotImplementedException();
    }
}