using System.Net;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using Npgsql;
using test.models;

namespace test.integration.group;

public class InviteUser
{
    private HttpClient _httpClient;
    
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task InviteUserTest()
    {
        try
        {
            string email = "owner@example.com";
            
            string? token = await Helper.Authorize("owner@example.com");

            int ownerId = await GetUserId(email);
            
            User user = await CreateUser(
                "user@example.com",
                "Patrick Darling Andersen",
                55555555,
                DateTime.Now,
                "https://www.google.com");

            int groupId = await CreateGroup();

            await AddUserToGroup(ownerId, groupId, true);

            string url = "http://localhost:5100/api/group/invite";
            
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            GroupInvitation groupInvitation = new GroupInvitation()
            {
                GroupId = groupId,
                ReceiverId = user.Id
            };
            
            HttpResponseMessage resonseMessage = await _httpClient.PostAsJsonAsync(url, groupInvitation);

            TestContext.WriteLine("The full body response: " + await resonseMessage.Content.ReadAsStringAsync());
            
            using (new AssertionScope())
            {
                resonseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
    }

    private async Task<int> GetUserId(string email)
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
    
    private async Task<bool> AddUserToGroup(int userId, int groupId, bool owner)
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

    private async Task<User> CreateUser(
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
    
    private async Task<int> CreateGroup()
    {
        string name = "Group1";
        string description = "Description1";
        string imageUrl = "https://www.google.com";
        DateTime createdDate = DateTime.Now;
        
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
}