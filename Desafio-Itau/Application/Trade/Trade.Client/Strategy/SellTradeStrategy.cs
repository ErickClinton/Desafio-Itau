using DesafioInvestimentosItau.Application.Exceptions;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using DesafioInvestimentosItau.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Client.Strategy;

public class SellTradeStrategy : ITradeStrategy
{
    private readonly IUserService _userService;
    private readonly IQuoteService _quoteService;
    private readonly ITradeRepository _tradeRepository;
    private readonly IPositionService _positionService;
    private readonly ILogger _logger;

    public SellTradeStrategy(IUserService userService,
        IQuoteService quoteService,
        ITradeRepository tradeRepository,
        IPositionService positionService,
        ILogger<SellTradeStrategy> logger)
    {
        _userService = userService;
        _quoteService = quoteService;
        _tradeRepository = tradeRepository;
        _positionService = positionService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CreateTradeRequestDto createTradeRequestDto)
    {
        _logger.LogInformation($"Start service SellTradeStrategy - Request -  {createTradeRequestDto}");

        var user = await _userService.GetByIdAsync(createTradeRequestDto.UserId)
                   ?? throw new UserNotFoundException(createTradeRequestDto.UserId);

        var asset = await _quoteService.SearchQuote(createTradeRequestDto.AssetCode);

        var position = await _positionService.GetByUserAndAssetAsync(user.Id, asset.AssetCode)
                       ?? throw new Exception("User does not hold this asset");

        if (position.Quantity < createTradeRequestDto.Quantity)
            throw new ApplicationException("Not enough quantity to sell");

        var trade = new TradeEntity(
            user.Id,
            createTradeRequestDto.AssetCode,
            createTradeRequestDto.Quantity,
            createTradeRequestDto.UnitPrice,
            user.BrokerageFee,
            TradeTypeEnum.Sell
        );

        await _tradeRepository.CreateAsync(trade);

        var totalQtd = position.Quantity - createTradeRequestDto.Quantity;
        var profitLoss = (createTradeRequestDto.UnitPrice - position.AveragePrice) * createTradeRequestDto.Quantity;
        
        var totalValue = position.ProfitLoss + profitLoss;
        
        position.UpdatePositionSell(totalQtd, totalValue);

        await _positionService.UpdateAsync(position);
        
        _logger.LogInformation($"End service SellTradeStrategy");
    }
}
