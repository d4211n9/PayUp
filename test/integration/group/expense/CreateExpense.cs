namespace test.integration.group;

public class CreateExpense
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
    }
    
    
    
    
    
    
    
    
}