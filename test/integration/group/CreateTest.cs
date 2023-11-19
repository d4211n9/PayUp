using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;

namespace test.integration.group;

public class CreateTest
{
    private HttpClient _httpClient;
    
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    private async Task<string> Authorize(string email)
    {
        //Register
        var registration = new
        {
            Email = email,
            FullName = "fullName",
            Password = "password",
            PhoneNumber = "12345678",
            Created = DateTime.Now,
            ProfileUrl = "https://cdn-icons-png.flaticon.com/512/615/615075.png"
        };

        string urlReg = "http://localhost:5100/api/account/register";
        
        try
        {
            await _httpClient.PostAsJsonAsync(urlReg, registration);
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        
        //Login
        var login = new
        {
            Email = email,
            Password = "password",
        };
        
        string urlLogin = "http://localhost:5100/api/account/login";
        HttpResponseMessage response;
        
        try
        {
            response = await _httpClient.PostAsJsonAsync(urlLogin, login);
            
            var body = await response.Content.ReadAsStringAsync();
            var token = body.Substring(10, body.Length-12);
            return token;
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
    }
    
    [TestCase("test0@email.com", "Valid name", "Valid description", "https://cdn-icons-png.flaticon.com/512/615/615075.png", TestName = "Valid")]
    [TestCase("test1@email.com", "", "Valid description", "https://cdn-icons-png.flaticon.com/512/615/615075.png", TestName = "InvalidName")]
    [TestCase("test2@email.com", "Valid name", "", "https://cdn-icons-png.flaticon.com/512/615/615075.png", TestName = "InvalidDescription")]
    [TestCase("test3@email.com", "Valid name", "Valid description", "", TestName = "InvalidImage")]
    public async Task Create(
        string email,
        string name,
        string description,
        string imageUrl
    )
    {
        var token = await Authorize(email);
        
        var group = new
        {
            Name = name,
            Description = description,
            Image_Url = imageUrl,
            Created_Date = DateTime.UtcNow,
        };

        string url = "http://localhost:5100/api/group/create";
        HttpResponseMessage response;
        
        try
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            response = await _httpClient.PostAsJsonAsync(url, group);
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
                default:
                    response.IsSuccessStatusCode.Should().BeFalse();
                    break;
            }
        }
    }
}