using DesafioInvestimentosItau.Application.Exceptions;
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
        _logger.LogInformation($"Start service CreateAsync - Request - {position}");
        var newPosition = new PositionEntity()
        {
            UserId = position.UserId,
            AssetCode = position.AssetCode,
            Quantity = position.Quantity,
            AveragePrice = position.AveragePrice,
            ProfitLoss = 0
        };
        var response = await _positionRepository.CreateAsync(newPosition);
        _logger.LogInformation($"End service CreateAsync - Response - {response}");
        return response;
    }

    public async Task<AveragePriceResponse> GetAveragePriceAsync(string assetCode, long userId )
    {
        _logger.LogInformation($"Start service GetAveragePriceAsync - Request - {assetCode} - {userId}");
        
        var averagePrice = await _positionRepository.GetAveragePriceAsync(userId,assetCode);
        if(averagePrice == null) throw new AveragePriceException(assetCode,userId);
        
        var response = new AveragePriceResponse()
            { AssetCode = assetCode, AveragePrice = averagePrice.AveragePrice };
        _logger.LogInformation($"End service GetAveragePriceAsync - Response - {response}");
        return response;
    }
    
    public async Task UpdateAsync(PositionEntity position)
    {
        _logger.LogInformation($"Start service UpdateAsync - Request - {position}");
        await _positionRepository.UpdateAsync(position);
        _logger.LogInformation($"End service UpdateAsync");
    }
    
    public async Task<PositionEntity?> GetByUserAndAssetAsync(long userId, string assetCode)
    {
        _logger.LogInformation($"Start service GetByUserAndAssetAsync - Request - {userId}-{assetCode}");
        var response = await _positionRepository.GetByUserAndAssetAsync(userId, assetCode);
        _logger.LogInformation($"End service GetByUserAndAssetAsync - Response - {response}");
        return response;
    }

    public async Task<List<AssetPositionDto>> GetUserPositionsAsync(long userId)
    {
        _logger.LogInformation($"Start service GetUserPositionsAsync - Request - {userId}");
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

        _logger.LogInformation($"End service GetUserPositionsAsync - Response - {result}");

        return result;
    }

    public async Task<GlobalPositionDto> GetGlobalPositionAsync(long userId)
    {
        _logger.LogInformation($"Start service GetGlobalPositionAsync - Request - {userId}");
        var pl = await _positionRepository.GetTotalProfitLossAsync(userId);

        var globalPosition = new GlobalPositionDto
        {
            UserId = userId,
            TotalProfitLoss = pl

        };
        _logger.LogInformation($"End service GetGlobalPositionAsync - Response - {globalPosition}");

        return globalPosition;
    }

    public async Task<List<TopPositionDto>> GetTopUserPositionsAsync(int top)
    {
        _logger.LogInformation($"Start service GetGlobalPositionAsync - Request - {top}");
        var response = await _positionRepository.GetTopUserPositionsAsync(top);
        _logger.LogInformation($"End service GetGlobalPositionAsync - Response - {response}");
        return response;
    }
}