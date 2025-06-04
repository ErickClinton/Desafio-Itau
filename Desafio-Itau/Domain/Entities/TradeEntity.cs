using DesafioInvestimentosItau.Domain.Enums;

namespace DesafioInvestimentosItau.Domain.Entities;

public class TradeEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public TradeTypeEnum Type { get; set; }
    public decimal BrokerageFee { get; set; }
    public DateTime Timestamp { get; set; }

    public UserEntity? User { get; set; }
    public AssetEntity? Asset { get; set; }
}