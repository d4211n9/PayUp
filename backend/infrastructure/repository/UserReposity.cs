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
            INSERT INTO users.user (email, full_name, phone_number, created, profile_url) " +
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
        WHERE email = @id;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<User>(sql, new { id });
        }
    }
    
    public int GetIdByEmail(string email)
    {
        var sql = @"
        SELECT id FROM users.user
        WHERE email = @email;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<int>(sql, new { email });
        }
    }
}