using DesafioInvestimentosItau.Application.Exceptions;
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
        _logger.LogInformation($"Start service GetGroupedBuyTradesByUserAsync - Request - {userId}");
        
        var trades = await _tradeRepository.GetGroupedBuyTradesByUserAsync(userId);

        var grouped = trades
            .GroupBy(t => t.AssetCode)
            .Select(g => new GroupedTradesByAssetDto
            {
                AssetCode = g.Key,
                Trades = g.ToList()
            })
            .ToList();

        _logger.LogInformation($"End service GetGroupedBuyTradesByUserAsync - Response - {grouped}");
        return grouped;
    }

    public async Task CreateTrade(CreateTradeRequestDto createTradeRequestDto)
    {
        _logger.LogInformation($"Start service CreateTrade - Request -  {createTradeRequestDto}");
        var method = _tradeFactory.GetStrategy(createTradeRequestDto);
        await method.ExecuteAsync(createTradeRequestDto);
        _logger.LogInformation($"End CreateTrade");
    }

    public async Task<List<TradeEntity>> GetBuyTradesByUserAndAssetAsync(long userId, string assetCode)
    {
        _logger.LogInformation($"Start service GetBuyTradesByUserAndAssetAsync - Request - {userId}");
        var response = await _tradeRepository.GetBuyTradesByUserAndAssetAsync(userId, assetCode);
        _logger.LogInformation($"Start service GetBuyTradesByUserAndAssetAsync - Request - {userId}");

        return response;
    }
    
    public async Task<decimal> GetTotalBrokerageFeeAsync(long userId)
    {
        _logger.LogInformation($"Start service GetTotalBrokerageFeeAsync - Request - {userId}");
        var total = await _tradeRepository.GetTotalBrokerageFeeAsync(userId);
        _logger.LogInformation($"End service GetTotalBrokerageFeeAsync - Response - {total}");
        return total;
    }
    
    public async Task<decimal> GetTotalBrokerageAsync()
    {
        _logger.LogInformation($"Start service GetTotalBrokerageFeeAsync");
        var response = await _tradeRepository.GetTotalBrokerageAsync();
        _logger.LogInformation($"End service GetTotalBrokerageFeeAsync - Response - {response}");
        return response;
    }

    public async Task<List<TopBrokerageDto>> GetTopUserBrokeragesAsync(int top)
    {
        _logger.LogInformation($"Start service GetTopUserBrokeragesAsync");
        var response = await _tradeRepository.GetTopUserBrokeragesAsync(top);
        _logger.LogInformation($"End service GetTopUserBrokeragesAsync - Response - {response}");
        return response;
    }
    
    public async Task<decimal> CalculateAveragePrice(string assetCode)
    {
        _logger.LogInformation($"Start service CalculateAveragePrice - Request - {assetCode}");
        var trades = await _tradeRepository.GetBuyTradesByAssetAsync(assetCode);
        if (trades == null || !trades.Any())
            throw new NoTradesFoundException(assetCode);

        decimal totalValue = 0;
        int totalQuantity = 0;

        foreach (var trade in trades)
        {
            if (trade.Quantity <= 0 || trade.UnitPrice <= 0)
                throw new InvalidTradeDataException("Invalid trade data detected.");

            totalValue += trade.UnitPrice * trade.Quantity;
            totalQuantity += trade.Quantity;
        }

        if (totalQuantity == 0)
            throw new EmptyTradeQuantityException(assetCode);

        var response = totalValue / totalQuantity;
        _logger.LogInformation($"Start service CalculateAveragePrice - Response - {response}");
        return response;
    }
}