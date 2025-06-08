using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
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
    
    public async Task<PositionEntity> CreateAsync(PositionCreateDto position)
    {
        _logger.LogInformation("Creating position for AssetId: {AssetId}, UserId: {UserId}", position.AssetCode, position.UserId);
        var newPosition = new PositionEntity()
        {
            UserId = position.UserId,
            AssetCode = position.AssetCode,
            Quantity = position.Quantity,
            AveragePrice = position.AveragePrice,
            ProfitLoss = 0
        };
        return await _positionRepository.CreateAsync(newPosition);
    }

    public async Task<AveragePriceResponse> GetAveragePriceAsync(AveragePriceRequest averagePriceRequest)
    {
        var averagePrice = await _positionRepository.GetAveragePriceAsync(averagePriceRequest.UserId,averagePriceRequest.AssetCode);
        if(averagePrice == null) throw new ApplicationException("The average price was not found");
        return new AveragePriceResponse()
            { AssetCode = averagePriceRequest.AssetCode, AveragePrice = averagePrice.AveragePrice };
    }
    
    public async Task UpdateAsync(PositionEntity position)
    {
        _logger.LogInformation("Updating position with ID {PositionId}", position.Id);
        await _positionRepository.UpdateAsync(position);
        _logger.LogInformation("Position updated successfully.");
    }
    
    public async Task<PositionEntity?> GetByUserAndAssetAsync(long userId, string assetCode)
    {
        return await _positionRepository.GetByUserAndAssetAsync(userId, assetCode);
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
            AssetCode = p.AssetCode,
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