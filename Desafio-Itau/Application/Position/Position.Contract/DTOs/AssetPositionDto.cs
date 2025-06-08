namespace DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;

public class AssetPositionDto
{
    public string AssetCode { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }
}