using DesafioInvestimentosItau.Application.Investment.Investment.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DesafioInvestimentosItau.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class InvestmentsController : ControllerBase
{
    private readonly IInvestmentService _investmentService;
    private readonly ILogger<InvestmentsController> _logger;
    public InvestmentsController(IInvestmentService investmentService, ILogger<InvestmentsController> logger)
    {
        _investmentService = investmentService;
        _logger = logger;
    }

    [HttpGet("user/{userId}/total-invested")]
    public async Task<ActionResult<List<TotalInvestedByAssetDto>>> GetAllInvestmentsByUserId(long userId)
    {
        _logger.LogInformation($"Start method GetAllInvestmentsByUserId - Request - {userId}");
        var result = await _investmentService.GetAllInvestmentsByUserIdAsync(userId);
        return Ok(result);
    }
    
    [HttpGet("top/{top}")]
    public async Task<ActionResult<List<TotalInvestedByAssetDto>>> GetTopInvestments(int top)
    {
        _logger.LogInformation($"Start method GetUserPositions - GetTopInvestments - {top}");

        var result = await _investmentService.GetTopUserStatsAsync(top);
        return Ok(result);
    }
}
