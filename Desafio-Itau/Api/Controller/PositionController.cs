using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DesafioInvestimentosItau.Api.Controller;

[ApiController]
[Route("api/positions")]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;
    private readonly ILogger<PositionController> _logger;

    public PositionController(IPositionService positionService, ILogger<PositionController> logger)
    {
        _positionService = positionService;
        _logger = logger;
    }

    [HttpGet("average-price")]
    public async Task<ActionResult<AveragePriceResponse>> GetAveragePrice([FromQuery] string assetCode, [FromQuery] long userId)
    {
        _logger.LogInformation($"Start method GetAveragePrice - Request - {assetCode} - {userId}");
        var result = await _positionService.GetAveragePriceAsync(assetCode, userId);
        return Ok(result);
    }
    
    [HttpGet("user/{userId}/position")]
    public async Task<ActionResult<List<PositionEntity>>> GetAllPositionsByUserAsync(long userId)
    {
        _logger.LogInformation($"Start method GetAllPositionsByUserAsync - Request - {userId}");

        var allPositions = await _positionService.GetUserPositionsAsync(userId);
        return Ok(allPositions);
    }

    [HttpPost("find-position")]
    public async Task<ActionResult<PositionEntity?>> GetByUserAndAssetAsync(GetByUserAndAssetDto getByUserAndAssetDto)
    {
        _logger.LogInformation($"Start method GetByUserAndAssetAsync - Request - {getByUserAndAssetDto}");

        var result = await _positionService.GetByUserAndAssetAsync(getByUserAndAssetDto.UserId, getByUserAndAssetDto.AssetCode);
        if(result != null) return Ok(result);
        return NotFound(new { message = "User or asset not found" });
    }
}