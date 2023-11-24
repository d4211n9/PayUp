using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Npgsql;
using test.models;

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
        string searchQuery = "";
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
            int group1Id = CreateGroup();
            User owner = CreateUser(
                email,
                "Patrick Darling Andersen",
                55555555,
                DateTime.Now,
                "https://www.google.com");
            AddUserToGroup(owner.Id, group1Id, true);

            // Create group members
            members = AddMembers(group1Id);

            // Create invited members
            invited = AddInvitedUsers(group1Id, owner.Id);

            // Create invitable users 
            invitable = CreateUsers();

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            string searchParam = "searchquery=" + searchQuery + "&";
            string currentPageParam = "currentpage=" + currentPage + "&";
            string pageSizeParam = "pagesize=" + pageSize + "&";
            string groupIdParam = "groupid=" + group1Id;
            string url = "http://localhost:5100/api/user?" 
                + searchParam
                + currentPageParam
                + pageSizeParam;

            responseMessage = await _httpClient.GetAsync(url);

            string json = await responseMessage.Content.ReadAsStringAsync();

            responseUsers = JsonSerializer.Deserialize<IEnumerable<InvitableUser>>(json);
            
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

    private IEnumerable<User> CreateUsers()
    {
        User user1 = CreateUser(
            "userfgoag@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User user2 = CreateUser(
            "userkosfvdskvdsvps@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User user3 = CreateUser(
            "uservoskvdakfkaok@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");

        return new[] { user1, user2, user3 };
    }

    private IEnumerable<User> AddInvitedUsers(int groupId, int ownerId)
    {
        User invited1 = CreateUser(
            "userf@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User invited2 = CreateUser(
            "userkosfs@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User invited3 = CreateUser(
            "uservoskvdak@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        InviteUser(invited1.Id, groupId, ownerId, DateTime.Now);
        InviteUser(invited2.Id, groupId, ownerId, DateTime.Now);    
        InviteUser(invited3.Id, groupId, ownerId, DateTime.Now);
        
        return new[] { invited1, invited2, invited3 };
    }

    private User CreateUser(
        string email,
        string fullName,
        int phoneNumber,
        DateTime created,
        string profileUrl)
    {
        string sql = @"
                  INSERT INTO users.user (email, full_name, phone_number, created, profile_url) "" +
                  ""VALUES (@email, @fullName, @phoneNumber, @created, @profileUrl) "" +
                  ""RETURNING *";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return conn.QueryFirst<User>(sql, new
            {
                email,
                fullName,
                phoneNumber,
                created,
                profileUrl
            });
        }
    }

    private bool InviteUser(
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
            return conn.Execute(sql, new
            {
                receiverId,
                groupId,
                senderId,
                timeNow
            }) == 1;
        }
    }

    private IEnumerable<User> AddMembers(int groupId)
    {
        User member1 = CreateUser(
            "user1@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User member2 = CreateUser(
            "user3@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        User member3 = CreateUser(
            "user3@example.com", 
            "Patrick Darling Andersen",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        AddUserToGroup(member1.Id, groupId, false);
        AddUserToGroup(member2.Id, groupId, false);
        AddUserToGroup(member3.Id, groupId, false);

        return new[] { member1, member2, member3 };
    }
    
    private bool AddUserToGroup(int userId, int groupId, bool owner)
    {
        string sql = @"
                insert into groups.group_members (user_id, group_id, owner) 
            values (@userId, @groupId, @owner)";

        using (NpgsqlConnection conn = Helper.DataSource.OpenConnection())
        {
            return conn.Execute(sql, new
            {
                userId,
                groupId,
                owner
            }) == 1;
        }
    }

    private int CreateGroup()
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
            return conn.Execute(sql, new
            {
                name,
                description,
                imageUrl,
                createdDate
            });
        }
    }
}