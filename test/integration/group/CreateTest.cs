using System.Text;
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

    [TestCase("test0@email.com", "Valid name", "Valid description", TestName = "Valid")]
    [TestCase("test1@email.com", "", "Valid description", TestName = "InvalidName")]
    [TestCase("test2@email.com", "Valid name", "", TestName = "InvalidDescription")]
    //[TestCase("test3@email.com", "Valid name", "Valid description", TestName = "InvalidImage")]
    public async Task Create(
        string email,
        string name,
        string description
    )
    {
        var token = await Helper.Authorize(email);


        var formData = new MultipartFormDataContent()
        {
            { new StringContent(name), "Name" },
            { new StringContent(description), "Description" },
            { new StringContent("2023-11-21 10:48:24.584797"), "CreatedDate" },
        };

        formData.Add(new ByteArrayContent(CreateTestFormFile("wat")), "image");

        string url = "http://localhost:5100/api/group/create";
        HttpResponseMessage response;

        try
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            response = await _httpClient.PostAsync(url, formData);
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

    private byte[] CreateTestFormFile(string content)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);

        return bytes;
    }
}