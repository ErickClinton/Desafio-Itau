using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;

public interface IPositionRepository
{
    Task<IEnumerable<PositionEntity>> GetUserPositionsAsync(long userId);
    Task<decimal> GetTotalProfitLossAsync(long userId);
    Task<PositionEntity> CreateAsync(PositionEntity position);
    Task UpdateAsync(PositionEntity position);
    Task<PositionEntity?> GetByUserAndAssetAsync(long userId, long assetId);
    Task<PositionEntity?> GetAveragePriceAsync(long userId, string assetCode);

}