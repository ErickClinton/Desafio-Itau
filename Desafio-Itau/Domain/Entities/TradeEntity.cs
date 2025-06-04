using DesafioInvestimentosItau.Domain.Enums;

namespace DesafioInvestimentosItau.Domain.Entities;

public class TradeEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public TradeTypeEnum Type { get; set; }
    public decimal BrokerageFee { get; set; }
    public DateTime Timestamp { get; set; }

    public UserEntity? User { get; set; }
    public AssetEntity? Asset { get; set; }
}