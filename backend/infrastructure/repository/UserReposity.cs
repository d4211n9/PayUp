using api.models;
using Dapper;
using infrastructure.dataModels;
using Npgsql;


namespace infrastructure.repository;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }


    public User Create(RegisterModel model, DateTime created)
    {
        
        var sql = $@"
            INSERT INTO users.user (Email, FullName, PhoneNumber, Created, ProfileUrl) " +
                  "VALUES (@Email, @FullName, @PhoneNumber, @Created, @ProfileUrl) " +
                  "RETURNING *";

        var user = new RegisterModel
        {
            Email = model.Email,
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            Created = created,
            ProfileUrl = model.ProfileUrl,
            Password = null,
        };
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<User>(sql, user);
        }
    }

    public User? GetById(string id)
    {
        var sql = @"
        SELECT * FROM users.user
        WHERE Id = @id;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<User>(sql, new { id });
        }
    }
    
    public int GetByEmail(string email)
    {
        var sql = @"
        SELECT Id FROM users.user
        WHERE email = @email;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<int>(sql, new { email });
        }
    }
}