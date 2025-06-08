using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;

public interface IPositionService
{
    Task<List<AssetPositionDto>> GetUserPositionsAsync(long userId);
    Task<GlobalPositionDto> GetGlobalPositionAsync(long userId);
    Task<PositionEntity> CreateAsync(PositionCreateDto position);
    Task UpdateAsync(PositionEntity position);
    
    Task<PositionEntity?> GetByUserAndAssetAsync(long userId, string assetCode);
    Task<AveragePriceResponse> GetAveragePriceAsync(AveragePriceRequest request);
    Task<List<TopPositionDto>> GetTopUserPositionsAsync(int top);
}