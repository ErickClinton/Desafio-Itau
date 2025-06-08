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
using DesafioInvestimentosItau.Application.Trade.Trade.Client.Strategy;
using FluentAssertions;

namespace DesafioInvestimentosItau.Tests;

public class SellTradeStrategyTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IQuoteService> _quoteServiceMock = new();
    private readonly Mock<ITradeRepository> _tradeRepositoryMock = new();
    private readonly Mock<IPositionService> _positionServiceMock = new();
    private readonly Mock<ILogger<SellTradeStrategy>> _loggerMock = new();
    private readonly SellTradeStrategy _strategy;

    public SellTradeStrategyTests()
    {
        _strategy = new SellTradeStrategy(
            _userServiceMock.Object,
            _quoteServiceMock.Object,
            _tradeRepositoryMock.Object,
            _positionServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateTradeAndUpdatePosition_WhenValid()
    {
        var dto = new CreateTradeRequestDto
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 5,
            UnitPrice = 30,
            Modality = TradeTypeEnum.Sell
        };

        var user = new UserEntity { Id = 1, BrokerageFee = 2 };
        var quote = new QuotationMessageDto { AssetCode = "PETR4", UnitPrice = 30, Timestamp = DateTime.UtcNow };
        var position = new PositionEntity
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 10,
            AveragePrice = 20,
            ProfitLoss = 0
        };

        _userServiceMock.Setup(u => u.GetByIdAsync(dto.UserId)).ReturnsAsync(user);
        _quoteServiceMock.Setup(q => q.SearchQuote(dto.AssetCode)).ReturnsAsync(quote);
        _positionServiceMock.Setup(p => p.GetByUserAndAssetAsync(user.Id, dto.AssetCode)).ReturnsAsync(position);
        _tradeRepositoryMock.Setup(t => t.CreateAsync(It.IsAny<TradeEntity>())).ReturnsAsync(new TradeEntity(1, "PETR4", 5, 30, 2, TradeTypeEnum.Sell));

        await _strategy.ExecuteAsync(dto);

        _positionServiceMock.Verify(p => p.UpdateAsync(It.IsAny<PositionEntity>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenNotEnoughQuantity()
    {
        var dto = new CreateTradeRequestDto
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 20,
            UnitPrice = 30,
            Modality = TradeTypeEnum.Sell
        };

        var user = new UserEntity { Id = 1, BrokerageFee = 2 };
        var quote = new QuotationMessageDto { AssetCode = "PETR4", UnitPrice = 30, Timestamp = DateTime.UtcNow };
        var position = new PositionEntity
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 5,
            AveragePrice = 20,
            ProfitLoss = 0
        };

        _userServiceMock.Setup(u => u.GetByIdAsync(dto.UserId)).ReturnsAsync(user);
        _quoteServiceMock.Setup(q => q.SearchQuote(dto.AssetCode)).ReturnsAsync(quote);
        _positionServiceMock.Setup(p => p.GetByUserAndAssetAsync(user.Id, dto.AssetCode)).ReturnsAsync(position);

        var act = async () => await _strategy.ExecuteAsync(dto);

        await act.Should().ThrowAsync<ApplicationException>().WithMessage("*Not enough quantity*");
    }
}
