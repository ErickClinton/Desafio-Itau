using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface ITradeService
{
    Task<decimal> GetTotalBrokerageFeeAsync(long userId);
    Task<List<GroupedTradesByAssetDto>> GetGroupedBuyTradesByUserAsync(long userId);

}