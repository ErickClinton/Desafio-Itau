using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Client;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IPositionService _positionService;
    private readonly ILogger<TradeService> _logger;
    private readonly ITradeFactory _tradeFactory;

    public TradeService(ITradeRepository tradeRepository,ILogger<TradeService> logger, IPositionService positionService,ITradeFactory tradeFactory)
    {
        _tradeRepository = tradeRepository;
        _logger = logger;
        _positionService = positionService;
        _tradeFactory = tradeFactory;
    }

    public async Task<List<GroupedTradesByAssetDto>> GetGroupedBuyTradesByUserAsync(long userId)
    {
        _logger.LogInformation("Start GetGroupedBuyTradesByUserAsync - UserId: {UserId}", userId);
        
        var trades = await _tradeRepository.GetGroupedBuyTradesByUserAsync(userId);

        var grouped = trades
            .GroupBy(t => t.AssetCode)
            .Select(g => new GroupedTradesByAssetDto
            {
                AssetCode = g.Key,
                Trades = g.ToList()
            })
            .ToList();

        _logger.LogInformation("End GetGroupedBuyTradesByUserAsync - Found {Count} asset groups", grouped.Count);
        return grouped;
    }

    public async Task CreateTrade(CreateTradeRequestDto createTradeRequestDto)
    {
        _logger.LogInformation("Start GetGroupedBuyTradesByUserAsync - Request -  {CreateTradeRequest}", createTradeRequestDto);
        var method = _tradeFactory.GetStrategy(createTradeRequestDto);
        await method.ExecuteAsync(createTradeRequestDto);

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
    
    public async Task<decimal> GetTotalBrokerageAsync()
    {
        return await _tradeRepository.GetTotalBrokerageAsync();
    }

    public async Task<List<TopBrokerageDto>> GetTopUserBrokeragesAsync(int top)
    {
        return await _tradeRepository.GetTopUserBrokeragesAsync(top);
    }
}