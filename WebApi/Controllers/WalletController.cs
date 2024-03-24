using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers;

[Route("api/wallet")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public ActionResult<Wallet> Get()
    {
        var wallet = _walletService.Wallet;

        return Ok(wallet);
    }

    [HttpPut("add-funds")]
    public ActionResult<Wallet> AddFunds([FromBody] decimal amount)
    {
        var wallet = _walletService.Wallet;
        wallet.AddFunds(amount);

        return Ok(wallet);
    }

    [HttpPut("remove-funds")]
    public ActionResult<Wallet> RemoveFunds([FromBody] decimal amount)
    {
        var wallet = _walletService.Wallet;
        wallet.RemoveFunds(amount);

        return Ok(wallet);
    }
}
