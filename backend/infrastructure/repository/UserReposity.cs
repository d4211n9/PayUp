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
        using var conn = _dataSource.OpenConnection();
        return conn.QueryFirst<User>(sql, user);
    }

    public User? GetById(int userId)
    {
        var sql = @$" 
SELECT
    id as {nameof(User.Id)},
    email as {nameof(User.Email)},
    full_name as {nameof(User.FullName)},
    phone_number as {nameof(User.PhoneNumber)},
    created as Created,
    profile_url as ProfileUrl
        FROM users.user
        WHERE id = @userId;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<User>(sql, new { userId });
        }
    }
    
    public User? GetByEmail(string email)
    {
        var sql = @"
        SELECT * FROM users.user
        WHERE email = @email;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<User>(sql, new { email });
        }
    }

    public User EditUserInfo(UserInfoDto user)
    {
        //todo edit name email and phone.. return all user info or should we even do that 
        throw new NotImplementedException("is not implemented in user repo..");
    }

    public bool DeleteUser(int userId)
    {
        //todo delete user and return true if user is successfully deleted. 
        throw new NotImplementedException("not implemented in the user repo");
    }
}