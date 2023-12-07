using Dapper;
using Npgsql;
using test.models;

namespace test;

public class SqlHelper
{
    private HttpClient _httpClient;

    public SqlHelper()
    {
        _httpClient = new HttpClient();
    }
    
    /**----------------------------------------------------------------------------------------------------------------------------------------------------
     * User relevant 
     */
    public async Task<User> CreateUser(
        string email,
        string fullName,
        int phoneNumber,
        DateTime created,
        string profileUrl)
    {
        string sql = @"
                  INSERT INTO users.user (email, full_name, phone_number, created, profile_url)
                  VALUES (@email, @fullName, @phoneNumber, @created, @profileUrl)
                  RETURNING id, email, full_name AS FullName, phone_number AS PhoneNumber, created, profile_url AS ProfileUrl";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return await Task.FromResult(conn.QueryFirst<User>(sql, new
            {
                email,
                fullName,
                phoneNumber,
                created,
                profileUrl
            }));
        }
    }
    
    
    public async Task<int> GetUserId(string email)
    {
        string sql = @"
                    SELECT users.user.id
                    FROM users.user
                    WHERE users.user.email = @email";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return await Task.FromResult(conn.QuerySingleOrDefault<int>(sql, new { email }));
        }
    }
    



    /**
     * Group relevant --------------------------------------------------------------------------------------------------------------------
     */
    
    public async Task<int> CreateGroup(string name, string description, string imageUrl, DateTime createdDate )
    {
         name = "Group1";
         description = "Description1"; 
         imageUrl = "https://www.google.com";
        createdDate = DateTime.Now;
        
        string sql = @"
                insert into groups.group (name, description, image_url, created_date) 
                values (@name, @description, @imageUrl, @createdDate) 
                returning groups.group.id;";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return await Task.FromResult(conn.Execute(sql, new
            {
                name,
                description,
                imageUrl,
                createdDate
            }));
        }
    }
    
    public async Task<bool> AddUserToGroup(int userId, int groupId, bool owner)
    {
        string sql = @"
                insert into groups.group_members (user_id, group_id, owner) 
            values (@userId, @groupId, @owner)";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return await Task.FromResult(conn.Execute(sql, new
            {
                userId,
                groupId,
                owner
            }) == 1);
        }
    }
    
    public bool InviteUserToGroup(FullGroupInvitation groupInvitation)
    {
        string sql = @"
                INSERT INTO groups.group_invitation
                (receiver_id, group_id, sender_id, date_received) 
                VALUES (@ReceiverId, @GroupId, @SenderId, @TimeNow);";
        
            using var conn = Helper.DataSource.OpenConnection();
            return conn.Execute(sql, new
            {
                groupInvitation.ReceiverId,
                groupInvitation.GroupId,
                groupInvitation.SenderId,
                TimeNow =  DateTime.Now
            }) == 1;
    }
    
}