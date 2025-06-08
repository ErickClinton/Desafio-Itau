using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client;
using Microsoft.AspNetCore.Mvc;

namespace DesafioInvestimentosItau.Api.Controller;

[ApiController]
[Route("trades")]
public class TradeController : ControllerBase
{
    private readonly ITradeService _tradeService;
    private readonly ILogger<TradeController> _logger;

    public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
    {
        _tradeService = tradeService;
        _logger = logger;
    }

    [HttpPost("buy")]
    public async Task<IActionResult> CreateBuyTrade([FromBody] CreateTradeRequestDto request)
    {
        _logger.LogInformation("Received request to buy trade for asset {AssetCode}", request.AssetCode);

        await _tradeService.CreateBuyTrade(request);

        return Ok(new { message = "Trade de compra registrada com sucesso." });
    }
}