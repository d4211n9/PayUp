using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace test.integration;

public class LoginTest
{
    
    private HttpClient _httpClient;
    
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [TestCase("test@email.dk", "123456789", TestName = "ValidPassword" )]
    [TestCase("test@email.dk", "1234567898810test", TestName = "InvalidPassword")]
    public async Task Login(
        string email,
        string password
    )
    {
        var login = new
        {
            Email = email,
            Password = password,
        };

        string regName = TestContext.CurrentContext.Test.Name;
        switch (regName)
        {
            case "InvalidPassword":
                password = "12345678910";
                break;
        }

        var registration = new
        {
            Email = email,
            FullName = "test testensen",
            Password = password,
            PhoneNumber = "88888888",
            Created = DateTime.Now,
            ProfileUrl = "https://ichef.bbci.co.uk/news/999/cpsprodpb/15951/production/_117310488_16.jpg"
        };
        
        //creates user
        const string registerUrl = "http://localhost:5100/api/account/register";

        try
        {
            await _httpClient.PostAsJsonAsync(registerUrl, registration);
        }
        catch (Exception e)
        {
            throw new Exception("could not register user");
        }
        
        
        
        
        
        string url = "http://localhost:5100/api/account/login";
        HttpResponseMessage response;
        
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, login);
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
                case "ValidPassword":
                    response.IsSuccessStatusCode.Should().BeTrue();
                    break;
                case "InvalidPassword":
                    response.IsSuccessStatusCode.Should().BeFalse();
                    break;
            }
        }
        
    }
}