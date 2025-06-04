namespace DesafioInvestimentosItau.Domain.Entities;

public class PositionEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }

    public UserEntity? User { get; set; }
    public AssetEntity? Asset { get; set; }
}