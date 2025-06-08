using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;

public interface ITradeService
{
    Task<decimal> GetTotalBrokerageFeeAsync(long userId);
    Task<List<GroupedTradesByAssetDto>> GetGroupedBuyTradesByUserAsync(long userId);
    Task<List<TradeEntity>> GetBuyTradesByUserAndAssetAsync(long userId, string assetCode);
    Task CreateTrade(CreateTradeRequestDto request);
    Task<decimal> GetTotalBrokerageAsync();
    Task<List<TopBrokerageDto>> GetTopUserBrokeragesAsync(int top);
    
}