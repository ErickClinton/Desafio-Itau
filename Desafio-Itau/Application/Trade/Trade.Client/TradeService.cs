using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using DesafioInvestimentosItau.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Client;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IUserService _userService;
    private readonly IPositionService _positionService;
    private readonly ILogger<TradeService> _logger;
    private readonly IQuoteService _quoteService;

    public TradeService(ITradeRepository tradeRepository,ILogger<TradeService> logger, 
        IUserService userService, IPositionService positionService,IQuoteService quoteService)
    {
        _tradeRepository = tradeRepository;
        _userService = userService;
        _logger = logger;
        _positionService = positionService;
        _quoteService = quoteService;
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

    public async Task CreateBuyTrade(CreateTradeRequestDto createTradeRequestDto)
    {
        _logger.LogInformation("Start GetGroupedBuyTradesByUserAsync - Request -  {CreateTradeRequest}", createTradeRequestDto);
        var user = await _userService.GetByIdAsync(createTradeRequestDto.UserId) ?? throw new Exception($"User {createTradeRequestDto.UserId} not found");
        var asset = await _quoteService.SearchQuote(createTradeRequestDto.AssetCode) ??
                    throw new Exception($"AssetCode {createTradeRequestDto.AssetCode} not found");

        var trade = new TradeEntity(
            user.Id,
            createTradeRequestDto.AssetCode,
            createTradeRequestDto.Quantity,
            asset.UnitPrice,
            user.BrokerageFee,
            TradeTypeEnum.Buy
        );

        await _tradeRepository.CreateAsync(trade);
        
        var position = await _positionService.GetByUserAndAssetAsync(user.Id, asset.AssetCode);

        var positionTask = position is not null
            ? UpdatePosition(position, createTradeRequestDto, user.BrokerageFee)
            : CreatePosition(createTradeRequestDto, user, asset.AssetCode);
        
        await positionTask;

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

    private async Task CreatePosition(CreateTradeRequestDto createTradeRequestDto, UserEntity user,string assetCode)
    {
        var avgPrice = (createTradeRequestDto.UnitPrice * createTradeRequestDto.Quantity + user.BrokerageFee) / createTradeRequestDto.Quantity;

        var newPosition = new PositionCreateDto()
        {
            UserId = user.Id,
            AssetCode = assetCode,
            Quantity = createTradeRequestDto.Quantity,
            AveragePrice = avgPrice,
            ProfitLoss = 0
        };
        await _positionService.CreateAsync(newPosition);
    }

    public async Task UpdatePosition(PositionEntity position,CreateTradeRequestDto createTradeRequestDto, decimal brokerageFee)
    {
        var totalQtd = position.Quantity + createTradeRequestDto.Quantity;
        var totalValue = (position.Quantity * position.AveragePrice) + (createTradeRequestDto.Quantity * createTradeRequestDto.UnitPrice + brokerageFee)/totalQtd;
        
        position.UpdatePosition(totalQtd, totalValue);
        await _positionService.UpdateAsync(position);
    }
}