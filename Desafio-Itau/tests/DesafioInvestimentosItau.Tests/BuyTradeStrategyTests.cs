using Xunit;
using Moq;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Enums;
using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Client.Strategy;

public class BuyTradeStrategyTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IQuoteService> _quoteServiceMock = new();
    private readonly Mock<ITradeRepository> _tradeRepositoryMock = new();
    private readonly Mock<IPositionService> _positionServiceMock = new();
    private readonly Mock<ILogger<BuyTradeStrategy>> _loggerMock = new();
    private readonly BuyTradeStrategy _strategy;

    public BuyTradeStrategyTests()
    {
        _strategy = new BuyTradeStrategy(
            _userServiceMock.Object,
            _quoteServiceMock.Object,
            _tradeRepositoryMock.Object,
            _positionServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateTradeAndPosition_WhenNoPositionExists()
    {
        var dto = new CreateTradeRequestDto
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 10,
            UnitPrice = 20,
            Modality = TradeTypeEnum.Buy
        };

        var user = new UserEntity { Id = 1, BrokerageFee = 2 };
        var quote = new QuotationMessageDto { AssetCode = "PETR4", UnitPrice = 20, Timestamp = DateTime.UtcNow };

        _userServiceMock.Setup(u => u.GetByIdAsync(dto.UserId)).ReturnsAsync(user);
        _quoteServiceMock.Setup(q => q.SearchQuote(dto.AssetCode)).ReturnsAsync(quote);
        _tradeRepositoryMock.Setup(t => t.CreateAsync(It.IsAny<TradeEntity>())).ReturnsAsync(new TradeEntity(1, "PETR4", 10, 20, 2, TradeTypeEnum.Buy));
        _positionServiceMock.Setup(p => p.GetByUserAndAssetAsync(user.Id, dto.AssetCode)).ReturnsAsync((PositionEntity?)null);

        await _strategy.ExecuteAsync(dto);

        _positionServiceMock.Verify(p => p.CreateAsync(It.IsAny<PositionCreateDto>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUpdatePosition_WhenPositionExists()
    {
        var dto = new CreateTradeRequestDto
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 10,
            UnitPrice = 20,
            Modality = TradeTypeEnum.Buy
        };

        var user = new UserEntity { Id = 1, BrokerageFee = 2 };
        var quote = new QuotationMessageDto { AssetCode = "PETR4", UnitPrice = 20, Timestamp = DateTime.UtcNow };
        var existingPosition = new PositionEntity
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 10,
            AveragePrice = 20,
            ProfitLoss = 0
        };

        _userServiceMock.Setup(u => u.GetByIdAsync(dto.UserId)).ReturnsAsync(user);
        _quoteServiceMock.Setup(q => q.SearchQuote(dto.AssetCode)).ReturnsAsync(quote);
        _tradeRepositoryMock.Setup(t => t.CreateAsync(It.IsAny<TradeEntity>())).ReturnsAsync(new TradeEntity(1, "PETR4", 10, 20, 2, TradeTypeEnum.Buy));
        _positionServiceMock.Setup(p => p.GetByUserAndAssetAsync(user.Id, dto.AssetCode)).ReturnsAsync(existingPosition);

        await _strategy.ExecuteAsync(dto);

        _positionServiceMock.Verify(p => p.UpdateAsync(It.IsAny<PositionEntity>()), Times.Once);
    }
}
