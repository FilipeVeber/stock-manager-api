
using WebApi.FinnHub.Models;

namespace WebApi.ViewModels;

public record QuoteOutputViewModel(string Price, DateOnly Date)
{
    public static QuoteOutputViewModel FromFinnHubQuote(FinnHubQuote quote)
    {
        return new QuoteOutputViewModel(quote.CurrentPrice, ConvertQuoteTime(quote.TimeStamp));
    }

    private static DateOnly ConvertQuoteTime(double unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return DateOnly.FromDateTime(dateTime.AddSeconds(unixTimeStamp).ToLocalTime());
    }
}
