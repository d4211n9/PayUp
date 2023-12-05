using System.Text.Json;
using api.models;
using freecurrencyapi;

namespace infrastructure.repository;

public class CurrencyApiRepository
{
    private HttpClient _httpClient;
    private Freecurrencyapi _fx;
    private string _key = "fxa_live_yGJVJpOaY1JvlTcRhTz2w7FTcGOOPifYpRS4a4R7";

    public CurrencyApiRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    
    
    
    public string GetCurrencyList()
    {//todo should be a list of currencies instead of a string 
        _fx = new Freecurrencyapi("fxa_live_yGJVJpOaY1JvlTcRhTz2w7FTcGOOPifYpRS4a4R7");
        return _fx.Currencies("EUR,USD,CAD");
        
    }
}