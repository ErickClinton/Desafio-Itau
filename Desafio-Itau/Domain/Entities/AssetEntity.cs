namespace DesafioInvestimentosItau.Domain.Entities;

public class AssetEntity
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty;

    public List<TradeEntity> Trades { get; set; } = new();
    public List<Quote> Quotes { get; set; } = new();
    public List<PositionEntity> Positions { get; set; } = new();
}