using System.Data.SqlTypes;
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

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirst<User>(sql, user);
        }
        catch(Exception)
        {
            throw new SqlTypeException("could not create user");
        }
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
        
        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<User>(sql, new { userId });
        }
        catch
        {
            throw new SqlTypeException("Could not retrieve user");
        }
    }
    
    public User? GetByEmail(string email)
    {
        var sql = @"
        SELECT * FROM users.user
        WHERE email = @email;";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<User>(sql, new { email });
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not retrieve user");
        }
        
    }

    public User? EditUserInfo(UserInfoDto user)
    {
        var updateSql = @$"
        UPDATE users.user
        SET
            full_name = @{nameof(user.FullName)},
            email = @{nameof(user.Email)},
            phone_number = @{nameof(user.PhoneNumber)},
            profile_url = @{nameof(user.ProfileUrl)}
        WHERE id = @{nameof(user.Id)}
        RETURNING id as {nameof(User.Id)},
                  email as {nameof(User.Email)},
                  full_name as {nameof(User.FullName)},
                  phone_number as {nameof(User.PhoneNumber)},
                  created as Created,
                  profile_url as ProfileUrl;";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<User>(updateSql, user);
        }
        catch(Exception e)
        {
            throw new SqlTypeException("Could not update user");
        }
    }


    public bool DeleteUser(int userId)
    {
        throw new NotImplementedException("not implemented in repo");
        //todo should soft delete the user object
    }
}