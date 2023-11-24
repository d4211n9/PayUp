using System.Collections;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Update;
using Newtonsoft.Json;
using Npgsql;
using test.models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace test.integration.UserTest;

public class GetInvitableUsers
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task GetInvitableUsersTest()
    {
        string email = "owner@example.com";
        string searchQuery = "a";
        int currentPage = 0;
        int pageSize = 5;
        
        string? token = await Helper.Authorize(email);

        HttpResponseMessage responseMessage;
        IEnumerable<InvitableUser>? responseUsers;
        IEnumerable<User>? members;
        IEnumerable<User>? invited;
        IEnumerable<User>? invitable;

        try
        {
            // Create group and its owner
            int group1Id = await CreateGroup();
            int ownerId = await GetUserId(email);
            await AddUserToGroup(ownerId, group1Id, true);

            // Create group members
            members = await AddMembers(group1Id);

            // Create invited members
            invited = await AddInvitedUsers(group1Id, ownerId);

            // Create invitable users 
            invitable = await CreateUsers();

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            string searchParam = "searchquery=" + searchQuery + "&";
            string currentPageParam = "currentpage=" + currentPage + "&";
            string pageSizeParam = "pagesize=" + pageSize + "&";
            string groupIdParam = "groupid=" + group1Id;
            string url = "http://localhost:5100/api/user?" 
                + searchParam
                + currentPageParam
                + pageSizeParam
                + groupIdParam;

            responseMessage = await _httpClient.GetAsync(url);

            string json = await responseMessage.Content.ReadAsStringAsync();
            
            responseUsers = JsonConvert.DeserializeObject<IEnumerable<InvitableUser>>(json);
            
            TestContext.WriteLine("The full body response: "
                                  + json);
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        using (new AssertionScope())
        {
            responseMessage.IsSuccessStatusCode.Should().BeTrue();

            responseUsers.Should().NotBeNullOrEmpty();
            members.Should().NotBeNullOrEmpty();
            invited.Should().NotBeNullOrEmpty();
            invitable.Should().NotBeNullOrEmpty();

            IEnumerable<InvitableUser> inviteMembers =
                members.Select(user => new InvitableUser()
                {
                    FullName = user.FullName,
                    Id = user.Id,
                    ProfileUrl = user.ProfileUrl
                });
            
            IEnumerable<InvitableUser> inviteInvited =
                invited.Select(user => new InvitableUser()
                {
                    FullName = user.FullName,
                    Id = user.Id,
                    ProfileUrl = user.ProfileUrl
                });
            
            IEnumerable<InvitableUser> inviteInvitable =
                invitable.Select(user => new InvitableUser()
                {
                    FullName = user.FullName,
                    Id = user.Id,
                    ProfileUrl = user.ProfileUrl
                });


            responseUsers.Should().NotContain(inviteMembers);
            responseUsers.Should().NotContain(inviteInvited);
            responseUsers.Should().Contain(inviteInvitable);
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

    private async Task<List<User>> CreateUsers()
    {
        User user1 = await CreateUser(
            "userfgoag@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User user2 = await CreateUser(
            "userkosfvdskvdsvps@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User user3 = await CreateUser(
            "uservoskvdakfkaok@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");

        return new List<User>(new[] { user1, user2, user3 });
    }

    private async Task<List<User>> AddInvitedUsers(int groupId, int ownerId)
    {
        User invited1 = await CreateUser(
            "userf@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User invited2 = await CreateUser(
            "userkosfs@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User invited3 = await CreateUser(
            "uservoskvdak@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        InviteUser(invited1.Id, groupId, ownerId, DateTime.Now);
        InviteUser(invited2.Id, groupId, ownerId, DateTime.Now);    
        InviteUser(invited3.Id, groupId, ownerId, DateTime.Now);
        
        return new List<User>(new[] { invited1, invited2, invited3 });
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
                  RETURNING email, full_name AS FullName, phone_number AS PhoneNumber, created, profile_url AS ProfileUrl";

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

    private async Task<bool> InviteUser(
        int receiverId,
        int groupId,
        int senderId,
        DateTime timeNow)
    {
        string sql = @"
                INSERT INTO groups.group_invitation
                (receiver_id, group_id, sender_id, date_received) 
                VALUES (@receiverId, @groupId, @senderId, @timeNow);";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return await Task.FromResult(conn.Execute(sql, new
            {
                receiverId,
                groupId,
                senderId,
                timeNow
            }) == 1);
        }
    }

    private async Task<List<User>> AddMembers(int groupId)
    {
        User member1 = await CreateUser(
            "user1@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User member2 = await CreateUser(
            "user3opgogpsa@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User member3 = await CreateUser(
            "user3@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        AddUserToGroup(member1.Id, groupId, false);
        AddUserToGroup(member2.Id, groupId, false);
        AddUserToGroup(member3.Id, groupId, false);

        return new List<User>(new[] { member1, member2, member3 });
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