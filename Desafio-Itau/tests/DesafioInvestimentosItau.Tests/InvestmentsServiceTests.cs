using Xunit;
using Moq;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Enums;
using DesafioInvestimentosItau.Application.Exceptions;
using DesafioInvestimentosItau.Application.Investment.Investment.Client;
using DesafioInvestimentosItau.Application.Investment.Investment.Contract.DTOs;
using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using FluentAssertions;

namespace DesafioInvestimentosItau.Tests;

public class InvestmentServiceTests
{
    private readonly Mock<ITradeService> _tradeServiceMock;
    private readonly Mock<IPositionService> _positionServiceMock;
    private readonly Mock<ILogger<InvestmentService>> _loggerMock;
    private readonly InvestmentService _investmentService;

    public InvestmentServiceTests()
    {
        _tradeServiceMock = new Mock<ITradeService>();
        _positionServiceMock = new Mock<IPositionService>();
        _loggerMock = new Mock<ILogger<InvestmentService>>();

        _investmentService = new InvestmentService(
            _tradeServiceMock.Object,
            _positionServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetAllInvestmentsByUserIdAsync_ShouldReturnCorrectTotals()
    {
        var userId = 1L;
        var trades = new List<TradeEntity>
        {
            new TradeEntity(userId, "PETR4", 10, 10, 1, TradeTypeEnum.Buy),
            new TradeEntity(userId, "PETR4", 5, 12, 1, TradeTypeEnum.Buy),
            new TradeEntity(userId, "ITUB4", 8, 20, 1, TradeTypeEnum.Buy)
        };

        _tradeServiceMock.Setup(t => t.GetGroupedBuyTradesByUserAsync(userId)).ReturnsAsync(
            new List<GroupedTradesByAssetDto>
            {
                new GroupedTradesByAssetDto
                {
                    AssetCode = "PETR4",
                    Trades = trades.FindAll(t => t.AssetCode == "PETR4")
                },
                new GroupedTradesByAssetDto
                {
                    AssetCode = "ITUB4",
                    Trades = trades.FindAll(t => t.AssetCode == "ITUB4")
                }
            });

        var result = await _investmentService.GetAllInvestmentsByUserIdAsync(userId);

        result.Should().HaveCount(2);
        result.Should().Contain(i => i.AssetCode == "PETR4" && i.TotalInvest == (10*10 + 1 + 5*12 + 1));
        result.Should().Contain(i => i.AssetCode == "ITUB4" && i.TotalInvest == (8*20 + 1));
    }

    [Fact]
    public async Task GetTopUserStatsAsync_ShouldReturnTopData()
    {
        var top = 2;
        _positionServiceMock.Setup(p => p.GetTopUserPositionsAsync(top))
            .ReturnsAsync(new List<TopPositionDto>
            {
                new TopPositionDto { UserId = 1, UserName = "Ana", Email = "ana@test.com", TotalQuantity = 100, TotalValue = 1000 }
            });

        _tradeServiceMock.Setup(t => t.GetTopUserBrokeragesAsync(top)).ReturnsAsync(new List<TopBrokerageDto>
        {
            new TopBrokerageDto { UserId = 2, UserName = "Bob", Email = "bob@test.com", TotalBrokerage = 500.25m }
        });

        var result = await _investmentService.GetTopUserStatsAsync(top);

        result.Should().NotBeNull();
        result.TopByQuantity.Should().ContainSingle(q => q.UserId == 1);
        result.TopByBrokerage.Should().ContainSingle(b => b.UserId == 2);
    }

    [Fact]
    public async Task GetTopUserStatsAsync_ShouldThrow_WhenTopIsZero()
    {
        var act = async () => await _investmentService.GetTopUserStatsAsync(0);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*greater than zero*");
    }

    [Fact]
    public async Task GetTopUserStatsAsync_ShouldThrow_WhenNoData()
    {
        var top = 5;
        _positionServiceMock.Setup(p => p.GetTopUserPositionsAsync(top)).ReturnsAsync(new List<TopPositionDto>());
        _tradeServiceMock.Setup(t => t.GetTopUserBrokeragesAsync(top)).ReturnsAsync(new List<TopBrokerageDto>());

        var act = async () => await _investmentService.GetTopUserStatsAsync(top);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*No investment data*");
    }
}
