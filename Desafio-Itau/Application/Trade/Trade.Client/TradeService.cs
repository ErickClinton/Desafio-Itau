using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Client;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly ILogger<TradeService> _logger;

    public TradeService(ITradeRepository tradeRepository,ILogger<TradeService> logger)
    {
        _tradeRepository = tradeRepository;
        _logger = logger;
    }

    public async Task<List<GroupedTradesByAssetDto>> GetGroupedBuyTradesByUserAsync(long userId)
    {
        _logger.LogInformation("Start GetGroupedBuyTradesByUserAsync - UserId: {UserId}", userId);
        
        var trades = await _tradeRepository.GetGroupedBuyTradesByUserAsync(userId);

        var grouped = trades
            .GroupBy(t => t.Asset.Code)
            .Select(g => new GroupedTradesByAssetDto
            {
                AssetCode = g.Key,
                Trades = g.ToList()
            })
            .ToList();

        _logger.LogInformation("End GetGroupedBuyTradesByUserAsync - Found {Count} asset groups", grouped.Count);
        return grouped;
    }

    public async Task<List<TradeEntity>> GetBuyTradesByUserAndAssetAsync(long userId, string assetCode)
    {
        return await _tradeRepository.GetBuyTradesByUserAndAssetAsync(userId, assetCode);
    }
    
    public async Task<decimal> GetTotalBrokerageFeeAsync(long userId)
    {
        _logger.LogInformation("Start method GetTotalBrokerageFeeAsync - Request - {UserId}", userId);
        var total = await _tradeRepository.GetTotalBrokerageFeeAsync(userId);
        _logger.LogInformation("End method GetTotalBrokerageFeeAsync - Response - {Total}", total);
        return total;
    }
}