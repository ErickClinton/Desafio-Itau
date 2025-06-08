using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;

public interface ITradeFactory
{
    ITradeStrategy GetStrategy(CreateTradeRequestDto dto);
}