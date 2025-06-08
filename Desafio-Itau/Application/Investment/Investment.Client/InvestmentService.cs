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
        _logger.LogInformation("Start GetAllInvestmentsByUserIdAsync - Request - {UserId}", userId);
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

        _logger.LogInformation("End GetAllInvestmentsByUserIdAsync - Response - {Count} ativos", result.Count);
        return result;
    }


    
    
    public async Task<AveragePriceByAssetDto> CalculateAveragePriceForUserAssetAsync(long userId, string assetCode)
    {
        _logger.LogInformation("Start CalculateAveragePriceForUserAssetAsync - UserId: {UserId}, Asset: {AssetCode}", userId, assetCode);

        var trades = await _tradeService.GetBuyTradesByUserAndAssetAsync(userId, assetCode);

        if (trades == null || !trades.Any())
            throw new ArgumentException("No buy trades found for the specified asset and user.");

        var totalQuantity = trades.Sum(t => t.Quantity);
        if (totalQuantity == 0)
            throw new InvalidOperationException("Total quantity must be greater than zero.");

        var totalValue = trades.Sum(t => t.UnitPrice * t.Quantity);
        var averagePrice = totalValue / totalQuantity;
        
        var price = new AveragePriceByAssetDto
        {
            AssetCode = assetCode,
            AveragePrice = averagePrice
        };
        
        _logger.LogInformation("End CalculateAveragePriceForUserAssetAsync - {Price}", price);

        return price;
    }

    

    // levar isso aqui para o position service e criar a controller
    public async Task<List<AssetPositionDto>> GetUserPositionsAsync(long userId)
    {
        _logger.LogInformation("Start GetUserPositionsAsync - Request - {UserId}", userId);
        var positions = (await _positionService.GetUserPositionsAsync(userId)).ToList();

        if (!positions.Any())
        {
            _logger.LogWarning("No positions found for user {UserId}", userId);
        }

        var result = positions.Select(p => new AssetPositionDto()
        {
            AssetCode = p.AssetCode,
            Quantity = p.Quantity,
            AveragePrice = p.AveragePrice,
            ProfitLoss = p.ProfitLoss
        }).ToList();

        _logger.LogInformation("End GetUserPositionsAsync - Response - {Count} posições", result.Count);
        return result;
    }
    
    public async Task<TopInvestmentsResponseDto> GetTopUserStatsAsync(int top)
    {
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

        return new TopInvestmentsResponseDto
        {
            TopByQuantity = topByQuantity,
            TopByBrokerage = topByBrokerage
        };
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
