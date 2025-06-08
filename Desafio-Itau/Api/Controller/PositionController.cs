using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioInvestimentosItau.Api.Controller;

[ApiController]
[Route("api/positions")]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;

    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpPost("average-price")]
    public async Task<ActionResult<AveragePriceResponse>> GetAveragePrice([FromBody] AveragePriceRequest request)
    {
        try
        {
            var result = await _positionService.GetAveragePriceAsync(request);
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}