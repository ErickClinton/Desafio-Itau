using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface ITradeRepository
{
    Task<decimal> GetTotalInvestedAsync(long userId);
    Task<decimal> GetTotalBrokerageFeeAsync(long userId);
    Task<List<TradeEntity>>GetGroupedBuyTradesByUserAsync(long userId);
}