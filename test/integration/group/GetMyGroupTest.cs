namespace test.integration.group;

public class GetGroupTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }
    
    
    
}