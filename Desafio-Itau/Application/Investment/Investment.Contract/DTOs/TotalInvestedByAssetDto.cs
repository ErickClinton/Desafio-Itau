namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

public class TotalInvestedByAssetDto
{
    public string AssetCode { get; set; } = null!;
    public decimal TotalInvest { get; set; }
}