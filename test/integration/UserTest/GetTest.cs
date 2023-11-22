using FluentAssertions;
using FluentAssertions.Execution;

namespace test.integration.UserTest;

public class GetTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [TestCase("teste@email.com", TestName = "Valid")]
    public async Task Get(
        string email
    ) {
        var user = new
        {
            Email = email,
        };
        
        var token = await Helper.Authorize(email);
        
        string url = "http://localhost:5100/api/user/currentuser";
        HttpResponseMessage response;

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

        using (new AssertionScope())
        {
            string testName = TestContext.CurrentContext.Test.Name;

            switch (testName)
            {
                case "Valid":
                    response.IsSuccessStatusCode.Should().BeTrue();
                    break;
            }

        }
    }

}