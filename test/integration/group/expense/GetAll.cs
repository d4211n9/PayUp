using FluentAssertions;
using FluentAssertions.Execution;

namespace test.integration.group.expense;

public class GetAll
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [TestCase("test0@email.com", 1, TestName = "Valid")]
    [TestCase("test1@email.com", 2, TestName = "Invalid")]
    public async Task GetAllExpenses(string email, int groupId)
    {
        // Arrange:
        // 1) Register test user and log in.

        // 2) Create groups with expenses.
        // User is only in one group and shouldn't have access to expenses from the other.

        var token = await Helper.Authorize(email);
        
        Helper.RunScript(Helper.ExpensesScript);

        string url = "http://localhost:5100/api/group/" + groupId + "/expenses";
        HttpResponseMessage response;

        // Act: Test the GetAllExpenses endpoint
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

        // Assert: Make sure authorized query gets a respond and unauthorized is denied
        using (new AssertionScope())
        {
            string testName = TestContext.CurrentContext.Test.Name;

            switch (testName)
            {
                case "Valid":
                    response.IsSuccessStatusCode.Should().BeTrue();
                    break;
                case "Invalid":
                    response.IsSuccessStatusCode.Should().BeFalse();
                    break;
            }
        }
    }
}