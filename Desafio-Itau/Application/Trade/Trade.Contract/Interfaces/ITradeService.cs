using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface ITradeService
{
    Task<decimal> GetTotalBrokerageFeeAsync(long userId);
    Task<List<GroupedTradesByAssetDto>> GetGroupedBuyTradesByUserAsync(long userId);
    Task<List<TradeEntity>> GetBuyTradesByUserAndAssetAsync(long userId, string assetCode);
    Task CreateBuyTrade(CreateTradeRequestDto dto);

}