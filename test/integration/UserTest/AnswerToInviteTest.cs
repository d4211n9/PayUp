using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using test.models;
namespace test.integration.group;
public class ReplyToInviteTest
{
    private HttpClient _httpClient;
    private SqlHelper _sqlHelper;
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _sqlHelper = new SqlHelper();
        Helper.TriggerRebuild();
    }
    [Test]
    public async Task ReplyInviteTest()
    {
        
        //user that invites
        var user = await _sqlHelper.CreateUser(
            "user2@example.com",
            "Im still Jenny",
            55555555,
            DateTime.Now,
            "https://www.google.com");
        
        //creates group and sets admin
        var group = await _sqlHelper.CreateGroup("group1", "describe the hell out of it ","https://www.google.com",   DateTime.Now);
        await _sqlHelper.AddUserToGroup(user.Id, group, true);
        
        //user that receives invite
        var user1 = await _sqlHelper.CreateUser(
            "user1@example.com",
            "Patrick Darling Andersen1",
            555555551,
            DateTime.Now,
            "https://www.google.com");
        
        
        //send invite to user1
        _sqlHelper.InviteUserToGroup(new FullGroupInvitation
        {
            GroupId = group,
            ReceiverId = user1.Id,
            SenderId = user.Id
        });
            
        
        //authenticates and builds the url
        string? token = await Helper.Authorize("user@example.com");
        string url = "http://localhost:5100/api/user/accept-invite";
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        
        Models.GroupInviteDto inviteAnswer = new Models.GroupInviteDto();
        inviteAnswer.Accepted = true;
        inviteAnswer.GroupId = group;

        
        //sends the request for adding a user to the group
        HttpResponseMessage resonseMessage = await _httpClient.PostAsJsonAsync(url, inviteAnswer);
        TestContext.WriteLine("The full body response: " + await resonseMessage.Content.ReadAsStringAsync());

        
        using (new AssertionScope())
        {
            resonseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        }


    }
}