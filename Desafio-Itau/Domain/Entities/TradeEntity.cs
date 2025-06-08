using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DesafioInvestimentosItau.Domain.Enums;

namespace DesafioInvestimentosItau.Domain.Entities;

public class TradeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long UserId { get; set; }
    public long AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public TradeTypeEnum Type { get; set; }
    public decimal BrokerageFee { get; set; }
    public DateTime Timestamp { get; set; }

    public UserEntity User { get; set; } = null!;
    public AssetEntity Asset { get; set; } = null!;
    
    public TradeEntity(long userId, long assetId, int quantity, decimal unitPrice, decimal brokerageFee, TradeTypeEnum type)
    {
        UserId = userId;
        AssetId = assetId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        BrokerageFee = brokerageFee;
        Type = type;
        Timestamp = DateTime.UtcNow;
    }
}