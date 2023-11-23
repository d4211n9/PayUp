using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;

namespace test.integration.group;

public class GetMyGroupsTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }
    
    [TestCase("test0@email.com", "test1@email.com", TestName = "Two Groups")]
    [TestCase("test1@email.com", "test0@email.com", TestName = "One Group")]
    public async Task GetMyGroups(string loggedInAccount, string secondAccount)
    {
        // Arrange:
        // 1) Register test user and log in.

        // 2) Create groups.
        // User is only member in 2 out of 3 groups and shouldn't get group no. 3 info.

        string testName = TestContext.CurrentContext.Test.Name;
        string token = "";

        switch (testName)
        {
            case "Two Groups":
                token = await Helper.Authorize(loggedInAccount);
                await Helper.Authorize(secondAccount);
                break;
            case "One Group":
                await Helper.Authorize(secondAccount);
                token = await Helper.Authorize(loggedInAccount);
                break;
        }
        
        
        Helper.RunScript(Helper.GroupsScript);

        string url = "http://localhost:5100/api/mygroups/";
        HttpResponseMessage response;

        // Act: Test the GetMyGroups endpoint
        try
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            response = await _httpClient.GetAsync(url);
            TestContext.WriteLine("The full body response: "
                                  + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        // Assert: Make sure query only responds with two groups
        using (new AssertionScope())
        {
            switch (testName)
            {
                case "Two Groups":
                    response.IsSuccessStatusCode.Should().BeTrue();
                    IEnumerable<Models.Group> groups = await response.Content.ReadFromJsonAsync<IEnumerable<Models.Group>>();
                    groups.Count().Should().Be(2);
                    break;
                case "One Group":
                    response.IsSuccessStatusCode.Should().BeTrue();
                    IEnumerable<Models.Group> group = await response.Content.ReadFromJsonAsync<IEnumerable<Models.Group>>();
                    group.Count().Should().Be(1);
                    break;
            }
        }
    }
}