using DesafioInvestimentosItau.Application.Trade.Trade.Client.Strategy;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Client;

public class TradeFactory : ITradeFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TradeFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITradeStrategy GetStrategy(CreateTradeRequestDto dto)
    {
        return dto.Modality switch
        {
            TradeTypeEnum.Buy => _serviceProvider.GetRequiredService<BuyTradeStrategy>(),
            TradeTypeEnum.Sell => _serviceProvider.GetRequiredService<SellTradeStrategy>(),
            _ => throw new ArgumentOutOfRangeException(nameof(dto.Modality), "Unknown trade modality")
        };
    }
}