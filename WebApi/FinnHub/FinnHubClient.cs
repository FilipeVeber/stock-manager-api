using WebApi.FinnHub.Models;

namespace WebApi.FinnHub;

public interface IFinnHubClient
{
    Task<FinnHubQuote> GetQuote(string symbol);
}

public class FinnHubClient : IFinnHubClient
{
    private readonly HttpClient _httpClient;

    public FinnHubClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FinnHubQuote> GetQuote(string symbol)
    {
        var response = await _httpClient.GetAsync($"https://finnhub.io/api/v1/quote?symbol={symbol}&token=cnuqeapr01qub9j001o0cnuqeapr01qub9j001og");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FinnHubQuote>(content);
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
