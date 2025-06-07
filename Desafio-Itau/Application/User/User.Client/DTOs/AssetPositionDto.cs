namespace DesafioInvestimentosItau.Application.User.User.Client.DTOs;

public class AssetPositionDto
{
    public string AssetCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }
}
