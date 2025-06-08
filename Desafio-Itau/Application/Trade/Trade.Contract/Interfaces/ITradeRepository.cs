using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;

public interface ITradeRepository
{
    Task<TradeEntity> CreateAsync(TradeEntity trade);
    Task<decimal> GetTotalInvestedAsync(long userId);
    Task<decimal> GetTotalBrokerageFeeAsync(long userId);
    Task<List<TradeEntity>>GetGroupedBuyTradesByUserAsync(long userId);
    Task<List<TradeEntity>> GetBuyTradesByUserAndAssetAsync(long userId, string assetCode);
}