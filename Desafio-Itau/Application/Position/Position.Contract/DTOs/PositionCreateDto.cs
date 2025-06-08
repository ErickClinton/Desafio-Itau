namespace DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;

public class PositionCreateDto
{
    public long UserId { get; set; }
    public long AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }
}