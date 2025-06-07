using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

public class GroupedTradesByAssetDto
{
    public string AssetCode { get; set; } = null!;
    public List<TradeEntity> Trades { get; set; }
}