using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using api.models;
using freecurrencyapi;
using freecurrencyapi.Helpers;

namespace infrastructure.repository;

public class CurrencyApiRepository
{
    private HttpClient _httpClient;
    private Freecurrencyapi _fx;
    private string _key = "fxa_live_yGJVJpOaY1JvlTcRhTz2w7FTcGOOPifYpRS4a4R7";
    public const string BaseUrl = "https://api.freecurrencyapi.com/v1/";

    public CurrencyApiRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }




    public async Task<ResponseObject> GetCurrencyList()
    {
        //todo should be a list of currencies instead of a string 
        var key = ("fxa_live_yGJVJpOaY1JvlTcRhTz2w7FTcGOOPifYpRS4a4R7");
        //  countryBody[0].currencies.Keys.First() 
        var currencyLookupUrl = "https://api.fxapi.com/v1/latest?" +
                                 "base_currency=DKK" +
                                "&apikey=" + key;
        var currencyResponse = await _httpClient.GetAsync(currencyLookupUrl);
        var asObject = JsonSerializer.Deserialize<ResponseObject>(await currencyResponse.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();

        Console.WriteLine(JsonSerializer.Serialize(asObject));
        return asObject;
    }
}