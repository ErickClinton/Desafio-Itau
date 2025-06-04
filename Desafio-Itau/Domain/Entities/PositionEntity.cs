namespace DesafioInvestimentosItau.Domain.Entities;

public class PositionEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }

    public UserEntity? User { get; set; }
    public AssetEntity? Asset { get; set; }
}