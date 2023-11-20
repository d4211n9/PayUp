using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;

namespace test.integration;

public class RegistrationTest
{
    private HttpClient _httpClient;
    
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }

    [TestCase("valid@email.com", "ValidName", "ValidPassword", "88888888",  "https://ichef.bbci.co.uk/news/999/cpsprodpb/15951/production/_117310488_16.jpg", TestName = "ValidRegistration")]
    [TestCase("valid2@email.com", "Valid Name", "invPw", "88888888", "https://ichef.bbci.co.uk/news/999/cpsprodpb/15951/production/_117310488_16.jpg", TestName = "InvalidPassword")]
    public async Task Register(
        string email,
        string fullName,
        string password,
        string phoneNumber,
        string profileUrl
        )
    {
        var registration = new
        {
            Email = email,
            FullName = fullName,
            Password = password,
            PhoneNumber = phoneNumber,
            Created = DateTime.Now,
            ProfileUrl = profileUrl
        };

        string url = "http://localhost:5100/api/account/register";
        HttpResponseMessage response;
        
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, registration);
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
                case "ValidRegistration":
                    response.IsSuccessStatusCode.Should().BeTrue();
                    break;
                case "InvalidPassword":
                    response.IsSuccessStatusCode.Should().BeFalse();
                    break;
            }
        }
    }
    
}