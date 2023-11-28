using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;

namespace test.integration.group;

public class GetBalances
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task GetBalancesTest()
    {
        var token = await Helper.Authorize("user@example.com");
        Helper.RunScript(Helper.BalanceScript);
        
        string url = "http://localhost:5100/api/group/1/balances";
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
            response.IsSuccessStatusCode.Should().BeTrue();
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Models.BalanceDto[]>(body);

            // As per Helper.BalanceScript:
            // User 1's balance should be -10
            // User 2's balance should be -20
            // User 3's balance should be +30
            
            foreach (var b in data)
            {
                if (b.UserId.Equals(1)) b.Amount.Should().Be(-10);
                if (b.UserId.Equals(2)) b.Amount.Should().Be(-20);
                if (b.UserId.Equals(3)) b.Amount.Should().Be(30);
            }
        }
    }
}