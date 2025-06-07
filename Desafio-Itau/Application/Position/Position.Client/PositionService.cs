using DesafioInvestimentosItau.Application.Position.Position.Contract;
using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Position.Position.Client;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _positionRepository;
    private readonly ILogger<PositionService> _logger;

    public PositionService(
        IPositionRepository positionRepository,
        ILogger<PositionService> logger)
    {
        _positionRepository = positionRepository;
        _logger = logger;
    }

    public async Task<List<AssetPositionDto>> GetUserPositionsAsync(long userId)
    {
        _logger.LogInformation("Start GetUserPositionsAsync - Request - {UserId}", userId);
        var positions = (await _positionRepository.GetUserPositionsAsync(userId)).ToList();

        if (!positions.Any())
        {
            _logger.LogWarning("No positions found for user {UserId}", userId);
        }

        var result = positions.Select(p => new AssetPositionDto
        {
            AssetCode = p.Asset.Code,
            Quantity = p.Quantity,
            AveragePrice = p.AveragePrice,
            ProfitLoss = p.ProfitLoss
        }).ToList();

        _logger.LogInformation("End GetUserPositionsAsync - Response - {Result}", result);

        return result;
    }

    public async Task<GlobalPositionDto> GetGlobalPositionAsync(long userId)
    {
        _logger.LogInformation("Start GetGlobalPositionAsync - Request - {UserId}", userId);
        var pl = await _positionRepository.GetTotalProfitLossAsync(userId);

        var globalPosition = new GlobalPositionDto
        {
            UserId = userId,
            TotalProfitLoss = pl

        };
        _logger.LogInformation("End GetGlobalPositionAsync - Response - {GlobalPosition}", globalPosition);

        return globalPosition;

    }
}