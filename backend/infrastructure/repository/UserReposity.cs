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


    public User Create(string fullName, string email, string phone, DateTime created, string profileUrl)
    {
        
        var sql = $@"
            INSERT INTO users.user (Email, FullName, PhoneNumber, Created, ProfileUrl) " +
                  "VALUES (@Email, @FullName, @PhoneNumber, @Created, @ProfileUrl) " +
                  "RETURNING Email, FullName, PhoneNumber, Created, ProfileUrl";

        var user = new User
        {
            Email = email,
            FullName = fullName,
            PhoneNumber = phone,
            Created = DateTime.Now,
            ProfileUrl = profileUrl
        };
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<User>(sql, new {user});
        }
    }

    public User? GetById(string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAll()
    {
        throw new NotImplementedException();
    }
}