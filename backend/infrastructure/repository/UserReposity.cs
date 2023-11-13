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
                  "RETURNING Email, FullName, PhoneNumber, Created, ProfileUrl";

        var user = new User
        {
            Email = model.Email,
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            Created = created,
            ProfileUrl = model.ProfileUrl
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
}