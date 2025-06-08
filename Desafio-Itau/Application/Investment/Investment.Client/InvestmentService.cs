using DesafioInvestimentosItau.Application.Investment.Investment.Contract.DTOs;
using DesafioInvestimentosItau.Application.Investment.Investment.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Investment.Investment.Client;

public class InvestmentService : IInvestmentService
{
    private readonly ITradeService _tradeService;
    private readonly IPositionService _positionService;
    private readonly ILogger<InvestmentService> _logger;

    public InvestmentService(
        ITradeService tradeRepository,
        IPositionService positionService,
        ILogger<InvestmentService> logger)
    {
        _tradeService = tradeRepository;
        _positionService = positionService;
        _logger = logger;
    }

    public async Task<List<TotalInvestedByAssetDto>> GetAllInvestmentsByUserIdAsync(long userId)
    {
        _logger.LogInformation($"Start service GetAllInvestmentsByUserIdAsync - Request - {userId}");
        var investedAssets = await _tradeService.GetGroupedBuyTradesByUserAsync(userId);
        var result = new List<TotalInvestedByAssetDto>();

        foreach (var assetGroup in investedAssets)
        {
            var total = CalculateTotalInvested(assetGroup.Trades);

            result.Add(new TotalInvestedByAssetDto
            {
                AssetCode = assetGroup.AssetCode,
                TotalInvest = total
            });
        }
        _logger.LogInformation($"End service GetAllInvestmentsByUserIdAsync - Response - {result}");
        return result;
    }
    
    public async Task<TopInvestmentsResponseDto> GetTopUserStatsAsync(int top)
    {
        _logger.LogInformation($"Start service GetTopUserStatsAsync - Request - {top}");
        var positions = await _positionService.GetTopUserPositionsAsync(top);
        var brokerages = await _tradeService.GetTopUserBrokeragesAsync(top);

        var topByQuantity = new List<TopPositionsResponseDto>();
        var topByBrokerage = new List<TopBrokerageFeeResponseDto>();

        foreach (var p in positions)
        {

            topByQuantity.Add(new TopPositionsResponseDto
            {
                UserId = p.UserId,
                Name = p.UserName,
                Email = p.Email,
                TotalQuantity = p.TotalQuantity,
                TotalValue = p.TotalValue
            });
        }

        foreach (var b in brokerages)
        {

            topByBrokerage.Add(new TopBrokerageFeeResponseDto
            {
                UserId = b.UserId,
                Name = b.UserName,
                Email = b.Email,
                TotalBrokerage = b.TotalBrokerage
            });
        }
        
        var response = new TopInvestmentsResponseDto
        {
            TopByQuantity = topByQuantity,
            TopByBrokerage = topByBrokerage
        };
        
        _logger.LogInformation($"End service GetAllInvestmentsByUserIdAsync - Response - {response}");
        return response;
    }

    private decimal CalculateTotalInvested(List<TradeEntity> trades)
    {
        decimal total = 0;

        foreach (var trade in trades)
        {
            total += (trade.UnitPrice * trade.Quantity) + trade.BrokerageFee;
        }

        return total;
    }
}
