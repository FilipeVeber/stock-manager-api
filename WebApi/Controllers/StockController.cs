using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApi.Data;
using WebApi.FinnHub;
using WebApi.FinnHub.Models;
using WebApi.ViewModels;

namespace WebApi.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IFinnHubClient _finnHubClient;
    private readonly WalletService _walletService;

    public StockController(IFinnHubClient finnHubClient, WalletService walletService)
    {
        _finnHubClient = finnHubClient;
        _walletService = walletService;
    }

    [HttpGet("quote/{symbol}")]
    public async Task<ActionResult<QuoteOutputViewModel>> GetQuoteAsync([FromRoute] string symbol)
    {
        var finnHubQuote = await GetQuoteFromFinnHubAsync(symbol.ToUpper());

        return Ok(QuoteOutputViewModel.FromFinnHubQuote(finnHubQuote));
    }

    [HttpPost("buy")]
    public async Task<ActionResult> BuyStockAsync([FromBody] TradeStockInputViewModel input)
    {
        if (input.Quantity <= 0)
        {
            return BadRequest("Please, input the quantity of stocks you want to buy.");
        }

        var sanitidezSymbol = input.Symbol.ToUpper();

        var finnHubQuote = await GetQuoteFromFinnHubAsync(sanitidezSymbol);
        var wallet = _walletService.Wallet;

        var sanitizedQuote = decimal.Parse(finnHubQuote.CurrentPrice, new CultureInfo("en-US"));

        decimal operationCost = sanitizedQuote * input.Quantity;

        if (operationCost > wallet.Balance)
        {
            return BadRequest("Insufficient funds. Please, add money to your wallet and try again.");
        }

        wallet.AddStock(sanitidezSymbol, input.Quantity, sanitizedQuote);

        return NoContent();
    }

    [HttpPost("sell")]
    public async Task<ActionResult> SeelStockAsync([FromBody] TradeStockInputViewModel input)
    {
        if (input.Quantity <= 0)
        {
            return BadRequest("Please, input the quantity of stocks you want to sell.");
        }

        var sanitidezSymbol = input.Symbol.ToUpper();

        var finnHubQuote = await GetQuoteFromFinnHubAsync(sanitidezSymbol);
        var wallet = _walletService.Wallet;

        if (wallet.Stocks.FirstOrDefault(x => x.Symbol.Equals(sanitidezSymbol)) is null)
        {
            return BadRequest("Stock was not found in your wallet. Please, inform a valid stock.");
        }

        var sanitizedQuote = decimal.Parse(finnHubQuote.CurrentPrice, new CultureInfo("en-US"));

        wallet.RemoveStock(sanitidezSymbol, input.Quantity, sanitizedQuote);

        return NoContent();
    }

    private async Task<FinnHubQuote> GetQuoteFromFinnHubAsync(string symbol)
    {
        return await _finnHubClient.GetQuote(symbol);
    }
}
