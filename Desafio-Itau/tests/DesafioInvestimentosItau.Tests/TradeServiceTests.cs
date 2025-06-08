using DesafioInvestimentosItau.Application.Exceptions;
using Xunit;
using Moq;
using FluentAssertions;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;
using DesafioInvestimentosItau.Application.Trade.Trade.Client;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Enums;

public class TradeServiceTests
{
    private readonly Mock<ITradeRepository> _tradeRepositoryMock;
    private readonly Mock<IPositionService> _positionServiceMock;
    private readonly Mock<ITradeFactory> _tradeFactoryMock;
    private readonly Mock<ILogger<TradeService>> _loggerMock;
    private readonly TradeService _tradeService;

    public TradeServiceTests()
    {
        _tradeRepositoryMock = new Mock<ITradeRepository>();
        _positionServiceMock = new Mock<IPositionService>();
        _loggerMock = new Mock<ILogger<TradeService>>();
        _tradeFactoryMock = new Mock<ITradeFactory>();

        _tradeService = new TradeService(
            _tradeRepositoryMock.Object,
            _loggerMock.Object,
            _positionServiceMock.Object,
            _tradeFactoryMock.Object
        );
    }

    [Fact]
    public async Task GetGroupedBuyTradesByUserAsync_ShouldReturnGroupedTrades()
    {
        var userId = 1L;
        var trades = new List<TradeEntity>
        {
            new TradeEntity(userId, "PETR4", 10, 20.5m, 1m, TradeTypeEnum.Buy),
            new TradeEntity(userId, "PETR4", 5, 21m, 1m, TradeTypeEnum.Buy),
            new TradeEntity(userId, "ITUB4", 7, 30m, 1m, TradeTypeEnum.Buy)
        };

        _tradeRepositoryMock.Setup(r => r.GetGroupedBuyTradesByUserAsync(userId)).ReturnsAsync(trades);

        var result = await _tradeService.GetGroupedBuyTradesByUserAsync(userId);

        result.Should().HaveCount(2);
        result.Should().Contain(x => x.AssetCode == "PETR4" && x.Trades.Count == 2);
        result.Should().Contain(x => x.AssetCode == "ITUB4" && x.Trades.Count == 1);
    }

    [Fact]
    public async Task CreateTrade_ShouldExecuteStrategy()
    {
        var dto = new CreateTradeRequestDto
        {
            UserId = 1,
            AssetCode = "PETR4",
            Quantity = 10,
            UnitPrice = 22.5m,
            Modality = TradeTypeEnum.Buy
        };

        var strategyMock = new Mock<ITradeStrategy>();
        _tradeFactoryMock.Setup(f => f.GetStrategy(dto)).Returns(strategyMock.Object);
        strategyMock.Setup(s => s.ExecuteAsync(dto)).Returns(Task.CompletedTask);

        await _tradeService.CreateTrade(dto);

        strategyMock.Verify(s => s.ExecuteAsync(dto), Times.Once);
    }

    [Fact]
    public async Task GetBuyTradesByUserAndAssetAsync_ShouldReturnTrades()
    {
        var userId = 1L;
        var assetCode = "VALE3";
        var trades = new List<TradeEntity> {
            new TradeEntity(userId, assetCode, 3, 50, 1, TradeTypeEnum.Buy)
        };

        _tradeRepositoryMock.Setup(r => r.GetBuyTradesByUserAndAssetAsync(userId, assetCode)).ReturnsAsync(trades);

        var result = await _tradeService.GetBuyTradesByUserAndAssetAsync(userId, assetCode);

        result.Should().HaveCount(1);
        result[0].AssetCode.Should().Be(assetCode);
    }

    [Fact]
    public async Task GetTotalBrokerageFeeAsync_ShouldReturnCorrectSum()
    {
        var userId = 1L;
        _tradeRepositoryMock.Setup(r => r.GetTotalBrokerageFeeAsync(userId)).ReturnsAsync(5.75m);

        var result = await _tradeService.GetTotalBrokerageFeeAsync(userId);

        result.Should().Be(5.75m);
    }

    [Fact]
    public async Task GetTotalBrokerageAsync_ShouldReturnSum()
    {
        _tradeRepositoryMock.Setup(r => r.GetTotalBrokerageAsync()).ReturnsAsync(42m);

        var result = await _tradeService.GetTotalBrokerageAsync();

        result.Should().Be(42m);
    }

    [Fact]
    public async Task GetTopUserBrokeragesAsync_ShouldReturnList()
    {
        var top = 3;
        var list = new List<TopBrokerageDto> {
            new TopBrokerageDto { UserId = 1, UserName = "João", Email = "j@a.com", TotalBrokerage = 9.9m }
        };

        _tradeRepositoryMock.Setup(r => r.GetTopUserBrokeragesAsync(top)).ReturnsAsync(list);

        var result = await _tradeService.GetTopUserBrokeragesAsync(top);

        result.Should().HaveCount(1);
        result[0].UserId.Should().Be(1);
    }

    [Fact]
    public async Task CalculateAveragePrice_ShouldReturnCorrectValue()
    {
        var trades = new List<TradeEntity> {
            new TradeEntity(1, "WEGE3", 10, 10, 1, TradeTypeEnum.Buy),
            new TradeEntity(1, "WEGE3", 10, 20, 1, TradeTypeEnum.Buy)
        };

        _tradeRepositoryMock.Setup(r => r.GetBuyTradesByAssetAsync("WEGE3")).ReturnsAsync(trades);

        var result = await _tradeService.CalculateAveragePrice("WEGE3");

        result.Should().BeApproximately(15, 0.01m);
    }

    [Fact]
    public async Task CalculateAveragePrice_ShouldThrow_WhenNoTrades()
    {
        _tradeRepositoryMock.Setup(r => r.GetBuyTradesByAssetAsync("FAIL3")).ReturnsAsync(new List<TradeEntity>());

        var act = async () => await _tradeService.CalculateAveragePrice("FAIL3");

        await act.Should().ThrowAsync<NoTradesFoundException>();
    }
}
