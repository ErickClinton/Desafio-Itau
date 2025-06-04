namespace DesafioInvestimentosItau.Domain.Entities;

public class UserEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal BrokerageFee { get; set; } 

    public List<TradeEntity> Trades { get; set; } = new();
    public List<PositionEntity> Positions { get; set; } = new();
}