using DesafioInvestimentosItau.Application.Exceptions;
using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using DesafioInvestimentosItau.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Client.Strategy;

public class BuyTradeStrategy : ITradeStrategy
{
    private readonly IUserService _userService;
    private readonly IQuoteService _quoteService;
    private readonly ITradeRepository _tradeRepository;
    private readonly IPositionService _positionService;
    private readonly ILogger _logger;

    public BuyTradeStrategy(
        IUserService userService,
        IQuoteService quoteService,
        ITradeRepository tradeRepository,
        IPositionService positionService,
        ILogger<BuyTradeStrategy> logger)
    {
        _userService = userService;
        _quoteService = quoteService;
        _tradeRepository = tradeRepository;
        _positionService = positionService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CreateTradeRequestDto dto)
    {
        _logger.LogInformation($"Start service BuyTradeStrategy - Request -  {dto}");

        var user = await _userService.GetByIdAsync(dto.UserId)
                   ?? throw new UserNotFoundException(dto.UserId);

        var asset = await _quoteService.SearchQuote(dto.AssetCode)
                    ?? throw new FallBackException(dto.AssetCode);

        var trade = new TradeEntity(
            user.Id,
            dto.AssetCode,
            dto.Quantity,
            dto.UnitPrice,
            user.BrokerageFee,
            TradeTypeEnum.Buy
        );

        await _tradeRepository.CreateAsync(trade);

        var position = await _positionService.GetByUserAndAssetAsync(user.Id, asset.AssetCode);

        var positionTask = position is not null
            ? UpdatePosition(position, dto, user.BrokerageFee)
            : CreatePosition(dto, user, asset.AssetCode);
        _logger.LogInformation($"End service BuyTradeStrategy");

    }
    private async Task CreatePosition(CreateTradeRequestDto createTradeRequestDto, UserEntity user,string assetCode)
    {
        _logger.LogInformation($"Start service CreatePosition - Request -  {createTradeRequestDto} - {user} - {assetCode}");
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
        _logger.LogInformation($"End service CreatePosition");
    }

    private async Task UpdatePosition(PositionEntity position,CreateTradeRequestDto createTradeRequestDto, decimal brokerageFee)
    {
        _logger.LogInformation($"Start service UpdatePosition - Request -  {position} - {createTradeRequestDto} - {brokerageFee}");
        var totalQtd = position.Quantity + createTradeRequestDto.Quantity;
        var totalValue = (position.Quantity * position.AveragePrice) + (createTradeRequestDto.Quantity * createTradeRequestDto.UnitPrice + brokerageFee)/totalQtd;
        if (totalQtd <= 0)
            throw new ArgumentException("Total quantity must be greater than zero.");
        position.UpdatePosition(totalQtd, totalValue);
        await _positionService.UpdateAsync(position);
        _logger.LogInformation($"End service UpdatePosition");
    }
}