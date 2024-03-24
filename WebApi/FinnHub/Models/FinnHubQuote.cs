using Newtonsoft.Json;

namespace WebApi.FinnHub.Models;

public class FinnHubQuote
{
    [JsonProperty("c")]
    public string CurrentPrice { get; set; }

    [JsonProperty("h")]
    public string HighPrice { get; set; }

    [JsonProperty("l")]
    public string LowPrice { get; set; }

    [JsonProperty("o")]
    public string OpenPrice { get; set; }

    [JsonProperty("pc")]
    public string PreviousClose { get; set; }

    [JsonProperty("t")]
    public int TimeStamp { get; set; }
}