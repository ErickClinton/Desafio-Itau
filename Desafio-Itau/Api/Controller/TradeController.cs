using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
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

    [HttpPost("")]
    public async Task<IActionResult> CreateBuyTrade([FromBody] CreateTradeRequestDto request)
    {
        _logger.LogInformation($"Start method CreateBuyTrade - Request - {request}");
        await _tradeService.CreateTrade(request);

        return Created(string.Empty, new { message = "Buy trade successfully recorded." });
    }
    
    [HttpGet("average-price-by-asset/{asset}")]
    public async Task<IActionResult> GetAveragePriceByAsset(string asset)
    {
        _logger.LogInformation($"Start method GetAveragePriceByAsset - Request - {asset}");

        var result = await _tradeService.CalculateAveragePrice(asset);

        return Ok(result);
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetTotalBrokerageFee(long userId)
    {
        _logger.LogInformation($"Start method GetTotalBrokerageFee - Request - {userId}");

        var result = await _tradeService.GetTotalBrokerageFeeAsync(userId);

        return Ok(result);
    }
    
    [HttpGet("brokerage/total")]
    public async Task<IActionResult> GetTotalBrokerage()
    {
        _logger.LogInformation($"Start method GetTotalBrokerage");

        var total = await _tradeService.GetTotalBrokerageAsync();
        return Ok(new { TotalBrokerage = total });
    }
}