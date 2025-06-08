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

    public InvestmentsController(IInvestmentService investmentService)
    {
        _investmentService = investmentService;
    }

    [HttpGet("user/{userId}/total-invested")]
    public async Task<ActionResult<List<TotalInvestedByAssetDto>>> GetAllInvestmentsByUserId(long userId)
    {
        var result = await _investmentService.GetAllInvestmentsByUserIdAsync(userId);
        return Ok(result);
    }

   
    [HttpGet("user/{userId}/positions")]
    public async Task<ActionResult<List<AssetPositionDto>>> GetUserPositions(long userId)
    {
        var result = await _investmentService.GetUserPositionsAsync(userId);
        return Ok(result);
    }
}
